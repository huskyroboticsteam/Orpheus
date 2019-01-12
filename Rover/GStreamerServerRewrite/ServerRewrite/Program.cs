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
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: <executable> <destination ip>");
                return;
            }

            DeviceMonitor devmon = InitializeGStreamer(args);

            if (devmon == null)
            {
                return;
            }

            PlatformID os = GetOS();

            Console.WriteLine("Serve Devices:");
            for (int i = 1; i - 1 < devmon.Devices.Length; i++)
            {
                Console.Write("[" + i + "] ");
                Devices.DumpDevice(devmon.Devices[i-1]);
            }

            String devicesToStart = Console.ReadLine();
            string[] deviceIndices = devicesToStart.Split(' ');

            for (int i = 0; i < deviceIndices.Length; i++)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() => Devices.StartDevice(devmon.Devices[Convert.ToInt32(deviceIndices[i]) - 1], args[0], os));
            }
            
            // Keep the program open until killed by user
            var loop = new GLib.MainLoop();
            loop.Run();
        }

        static PlatformID GetOS()
        {
            Console.WriteLine("Choose your OS: \n [1] Windows \n [2] Linux");

            GET_OS:
            int os = -1;
            try
            {
                String input = Console.ReadLine();
                os = Convert.ToInt32(input);
                if (os != 1 && os != 2)
                {
                    Console.WriteLine("Choose your OS: \n [1] Windows \n [2] Linux");
                    goto GET_OS;
                }
            }
            catch
            {
                Console.WriteLine("Choose your OS: \n [1] Windows \n [2] Linux");
                goto GET_OS;
            }

            return os == 1 ? PlatformID.Win32NT : PlatformID.Unix;
        }

        private static DeviceMonitor InitializeGStreamer(string[] args)
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
                return null;
            }

            return devmon;
        }
    }

    static class Devices
    {
        public static void StartDevice (Device d, String destinationIp, PlatformID id)
        {
            Pipeline pipeline = null;

            if (id == PlatformID.Unix)
            {
                pipeline = (Pipeline) PipelineFactory.GetLinuxPipeline(d, destinationIp);
            }
            else if (id == PlatformID.Win32NT)
            {
                pipeline = (Pipeline) PipelineFactory.GetWindowsPipeline(d, destinationIp);
            }

            if (pipeline == null)
            {
                Console.WriteLine("Pipeline not created");
                return;
            }

            StateChangeReturn ret = pipeline.SetState(State.Playing);
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
}
