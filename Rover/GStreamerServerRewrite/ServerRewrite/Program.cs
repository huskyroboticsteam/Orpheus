using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gst;

namespace ServerRewrite
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init(ref args);
            GtkSharp.GstreamerSharp.ObjectManager.Initialize();

            var devmon = new DeviceMonitor();

            // to show only cameras
            var caps = new Caps("video/x-raw");
            var filtId = devmon.AddFilter("Video/Source", caps);
            var bus = devmon.Bus;
            bus.AddWatch(Devices.OnBusMessage);
            if (!devmon.Start())
            {
                Console.WriteLine("Device monitor cannot start");
                return;
            }
            Console.WriteLine("Video devices count = " + devmon.Devices.Length);
            foreach (var dev in devmon.Devices)
            {
                Devices.DumpDevice(dev);
            }
            Devices.StartDevice(devmon.Devices[1], "192.168.0.5");
            //System.Threading.Tasks.Task.Factory.StartNew(() => Devices.StartDevice(devmon.Devices[0], "localhost"));
            var loop = new GLib.MainLoop();
            loop.Run();
        }
    }

    static class Devices
    {
        public static void StartDevice (Device d, String destinationIp)
        {
            Pipeline pipeline = (Pipeline) RTPPipeline.GetWindowsPipeline(d, destinationIp);
            StateChangeReturn ret = pipeline.SetState(State.Playing);
            Console.WriteLine(ret);
        }

        public static void DumpDevice(Device d)
        {
            Console.WriteLine($"{d.DeviceClass} : {d.DisplayName} : {d.Name}  : {GetWindowsDeviceIndex(d)} ");
        }

        public static int GetWindowsDeviceIndex(Device d)
        {
            return Convert.ToInt32(d.Name.Substring(8));
        }

        public static bool OnBusMessage(Bus bus, Message message)
        {
            switch (message.Type)
            {
                case MessageType.DeviceAdded:
                    {
                        var dev = message.ParseDeviceAdded();
                        Console.WriteLine("Device added: ");
                        DumpDevice(dev);
                        break;
                    }
                case MessageType.DeviceRemoved:
                    {
                        var dev = message.ParseDeviceRemoved();
                        Console.WriteLine("Device removed: ");
                        DumpDevice(dev);
                        break;
                    }
            }
            return true;
        }
    }

    public static class RTPPipeline
    {

        private static readonly Dictionary<String, StreamParameters> StreamParameters = new Dictionary<String, StreamParameters>
        {
            { "RICOH THETA S",  new StreamParameters(1280, 720, 0.5f, 5555) },
            { "ZED", new StreamParameters(3840, 1080, 0.25f, 5556) },
            { "USB 2.0 Camera", new StreamParameters(1280, 720, 0.25f, 5557) },
            { "Microsoft LifeCam Front", new StreamParameters(640, 480, 1.0f, 5558) },
            { "Microsoft LifeCam Rear", new StreamParameters(640, 480, 1.0f, 5559) },
            { "HD USB Camera", new StreamParameters(1920, 1080, 0.25f, 5560) }
        };

        // Windows:
        // ksvideosrc device-index=GetWindowsDeviceIndex() !
        // "video/x-raw, format=(string)I420, width=(int)1280, height=(int)720" ! 
        // openh264enc ! 
        // 'video/x-h264, stream-format=(string)byte-stream' ! 
        // h264parse !
        // rtph264pay ! 
        // udpsink host=192.168.0.5 port=5555
        public static Bin GetWindowsPipeline(Device dev, String destinationIp)
        {
            Element source = ElementFactory.Make("ksvideosrc");
            source["device-index"] = Devices.GetWindowsDeviceIndex(dev);

            Element encoder = ElementFactory.Make("openh264enc");

            return GetCommonPipeline(dev.DisplayName, destinationIp, source, "YUY2", encoder);
        }

        // Linux:
        // v4l2src device="/dev/video0" ! 
        // "video/x-raw, format=(string)I420, width=(int)1280, height=(int)720" ! 
        // omxh264enc ! 
        // 'video/x-h264, stream-format=(string)byte-stream' ! 
        // h264parse ! 
        // rtph264pay ! 
        // udpsink host=192.168.0.5 port=5555
        public static Bin GetLinuxPipeline(Device dev, String destinationIp)
        {
            Element source = ElementFactory.Make("v4l2src");
            source["device"] = dev.PathString;
            Element encoder = ElementFactory.Make("omxh264enc");

            Element sourceCap = ElementFactory.Make("capsfilter");
            sourceCap["caps"] = Caps.FromString("video/x-raw, format=(string)YUY2, width=(int)640, height(int)=480");

            return GetCommonPipeline(dev.Name, destinationIp, source, "I420", encoder);
        }

        private static Bin GetCommonPipeline(String name, String destinationIp, Element source, String sourceType, Element encoder)
        {
            Pipeline pipeline = new Pipeline();

            Element sourceCap = ElementFactory.Make("capsfilter");
            Element sourceCap2 = ElementFactory.Make("capsfilter");

            Element videoConvert = ElementFactory.Make("videoconvert");

            Element convertCap = ElementFactory.Make("capsfilter");
            Element encoderCap = ElementFactory.Make("capsfilter");
            Element rtpPay = ElementFactory.Make("rtph264pay");

            Element parser = ElementFactory.Make("h264parse");
            Element sink = ElementFactory.Make("udpsink");

            Element scale = ElementFactory.Make("videoscale");
            Element scaleCap = ElementFactory.Make("capsfilter");
            Element videoRate = ElementFactory.Make("videorate");
            Element videoCap = ElementFactory.Make("capsfilter");

            if (sourceCap == null || videoConvert == null 
                || convertCap == null || encoderCap == null || rtpPay == null || parser == null
                || sink == null || scale == null || scaleCap == null || videoRate == null
                || videoRate == null || videoCap == null)
            {
                Console.WriteLine("Failed to create an element");
            }

            int scaledWidth = (int)(StreamParameters[name].Width * StreamParameters[name].Scale);
            int scaledHeight = (int)(StreamParameters[name].Height * StreamParameters[name].Scale);

            sourceCap["caps"] = Caps.FromString("video/x-raw, format=(string)" + sourceType + ", width=(int)" + StreamParameters[name].Width + ", height=(int)" + StreamParameters[name].Height);

            sourceCap2["caps"] = Caps.FromString("video/x-raw, format=(string)" + sourceType + ", width=(int)" + scaledWidth + ", height=(int)" + scaledHeight);

            convertCap["caps"] = Caps.FromString("video/x-raw, format=(string)I420, width=(int)" 
                + scaledWidth + ", height=(int)" + scaledHeight);

            encoderCap["caps"] = Caps.FromString("video/x-h264, stream-format=(string)byte-stream");

            
            scaleCap["caps"] = Caps.FromString("video/x-raw, format=(string)" + sourceType + ", width=(int)"
                + scaledWidth + ", height=(int)" + scaledHeight);

            videoCap["caps"] = Caps.FromString("video/x-raw, framerate=10/1");

            sink["host"] = destinationIp;
            sink["port"] = StreamParameters[name].Port;            

            pipeline.Add(source, sourceCap, videoRate, videoCap, scale,
                scaleCap, sourceCap2, videoConvert, convertCap, encoder, encoderCap, parser, rtpPay, sink);
            if (!Element.Link(source, sourceCap, videoRate, videoCap, scale,
                scaleCap, sourceCap2, videoConvert, convertCap, encoder, encoderCap, parser, rtpPay, sink))
            {
                Console.WriteLine("Failed to Link");
            }

            pipeline.SetState(State.Null);

            return pipeline;
        }
    }
}
