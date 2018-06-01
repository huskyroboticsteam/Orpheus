using Gst;
using Gst.Video;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HuskyRobotics.UI.VideoStreamer
{
    /// <summary>
    /// Interaction logic for RTSPVideoWindow.xaml
    /// </summary>
    /// 
    // gst-launch-1.0 rtspsrc -v location = rtsp://192.168.0.42 user-id=admin user-pw=1234 latency=0 ! rtpjpegdepay ! jpegdec ! d3dvideosink
    public partial class RTSPVideoWindow : Window, VideoWindow
    {
        private Pipeline Pipeline;

        private Bin RTSP = (Bin) ElementFactory.Make("rtspsrc");
        private Element Depay = ElementFactory.Make("rtpjpegdepay");
        private Element Dec = ElementFactory.Make("jpegdec");
        private Element VideoSink = ElementFactory.Make("d3dvideosink");

        private int Port;
        private int BufferSizeMs;
        private string RecordingPath;
        private string _streamName;

        public string StreamName
        {
            get { return this._streamName; }
            set
            {
                this.Title = value;
                this._streamName = value;
            }
        }

        public RTSPVideoWindow(int Port, string StreamName, string RecordingPath, int BufferSizeMs = 200)
        {
            DataContext = this;
            this.Port = Port;
            this.StreamName = StreamName;
            this.BufferSizeMs = BufferSizeMs;
            this.RecordingPath = RecordingPath;

            Pipeline = new Pipeline();

            Window window = GetWindow(this);
            WindowInteropHelper wih = new WindowInteropHelper(this);
            wih.EnsureHandle(); // Generate Window Handle if current HWND is NULL (always occurs because background window)
            VideoOverlayAdapter overlay = new VideoOverlayAdapter(VideoSink.Handle);
            overlay.WindowHandle = wih.Handle;

            Pipeline["message-forward"] = true;
            RTSP["location"] = "rtsp://" + StreamName + ":8554/";
            RTSP["user-id"] = "admin";
            RTSP["user-pw"] = "1234";
            RTSP["latency"] = BufferSizeMs;


            RTSP.Add(Depay, Dec, VideoSink);
            Bin.Link(Depay, Dec, VideoSink);


            Pipeline.Add(RTSP);


            Pipeline.SetState(State.Null);

            Closing += OnCloseEvent;
            InitializeComponent();
        }

        /// <summary>
        /// Window (this) must be visible before this method is called.
        /// </summary>
        public void StartStream()
        {
            StateChangeReturn s = Pipeline.SetState(State.Playing);
        }

        private string GetFilename()
        {
            return "\\" + System.DateTime.Now.ToString("MM-dd-yyyy--HH;mm;ss") + " (" + this.StreamName + ")" + ".mp4";
        }

        private void OnCloseEvent(object sender, CancelEventArgs e)
        {
            // Properly shutdown down the sinks
            // We have to wait for EOS to propogate through the pipeline
            // Unclear how dependent delay is on machine's speed or other process usage
            Console.WriteLine("Shutting down video");
            Pipeline.SendEvent(Event.NewEos());
            Thread.Sleep(1000);

            // Cleanup the unmanaged class objects
            Pipeline.SetState(State.Null);
            Pipeline.Unref();
        }
    }
}
