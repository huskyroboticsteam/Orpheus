using System;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Gst;
using Gst.Video;
using System.Windows.Interop;
using System.Windows.Threading;

namespace HuskyRobotics.UI.VideoStreamer
{
    /// <summary>
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {
        private Pipeline Pipeline;
        private Element UDP = ElementFactory.Make("udpsrc");
        private Element JitterBuffer = ElementFactory.Make("rtpjitterbuffer");
        private Element Depay = ElementFactory.Make("rtph264depay");
        private Element Parse = ElementFactory.Make("h264parse");
        private Element Dec = ElementFactory.Make("openh264dec");
        private Element Sink = ElementFactory.Make("d3dvideosink");
        private Caps Caps = Caps.FromString("application/x-rtp, media=video, clock-rate=90000, encoding-name=H264");
        private int Port;
        private int Buffering;
        public string StreamName;

        // Stream with recording and showing: 
        // gst-launch-1.0 -vvv -e udpsrc caps=\"application/x-rtp, media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=5555 ! 
        // rtph264depay ! h264parse ! tee name=videoTee mp4mux name=mux ! filesink location=test.mp4 decodebin name=bin ! d3dvideosink videoTee. !
        // queue ! mux. videoTee. ! queue ! bin.


        /// <summary>
        /// A window that is separate from the main UI and plays a GStreamer RTP feed.
        /// </summary>
        /// <param name="Port"> The port that the RTP stream is sending data to. </param>
        /// <param name="Buffering"> The time in milliseconds of buffering of the feed. </param>
        public VideoWindow(int Port, string StreamName, int Buffering = 200)
        {
            this.Port = Port;
            this.StreamName = StreamName;
            this.Buffering = Buffering;

            Pipeline = new Pipeline();

            Window window = GetWindow(this);
            WindowInteropHelper wih = new WindowInteropHelper(this);
            wih.EnsureHandle(); // Generate Window Handle if current HWND is NULL (always occurs because background window)
            VideoOverlayAdapter overlay = new VideoOverlayAdapter(Sink.Handle);
            overlay.WindowHandle = wih.Handle;

            UDP["port"] = Port;
            UDP["caps"] = Caps;

            JitterBuffer["latency"] = Buffering;

            Pipeline.Add(UDP, JitterBuffer, Depay, Parse, Dec, Sink);
            Element.Link(UDP, JitterBuffer, Depay, Parse, Dec, Sink);

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

        private void OnCloseEvent(object sender, CancelEventArgs e)
        {
            // Cleanup the basically unmanaged class objects
            Pipeline.SetState(State.Null);
            Pipeline.Unref();
        }
    }
}
