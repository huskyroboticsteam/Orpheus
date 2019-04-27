using System;
using System.Windows;
using System.ComponentModel;
using Gst;
using Gst.Video;
using System.Windows.Interop;
using System.Threading;

namespace HuskyRobotics.UI.VideoStreamer
{
    /// <summary>
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class RTPVideoWindow : Window, VideoWindow
    {
        private Pipeline Pipeline;

        private Element UDP = ElementFactory.Make("udpsrc");
        private Element JitterBuffer = ElementFactory.Make("rtpjitterbuffer");
        private Element Depay = ElementFactory.Make("rtph264depay");
        private Element Parse = ElementFactory.Make("h264parse");

        private Element Mux = ElementFactory.Make("mp4mux");
        private Element FileSink = ElementFactory.Make("filesink");

        private Element Dec = ElementFactory.Make("avdec_h264");
        private Element VideoSink = ElementFactory.Make("d3dvideosink");

        private Element Tee = ElementFactory.Make("tee");
        private Element Q1 = ElementFactory.Make("queue");
        private Element Q2 = ElementFactory.Make("queue");

        private Caps Caps = Caps.FromString("application/x-rtp, media=video, clock-rate=90000, encoding-name=H264");

        private Element Filter = ElementFactory.Make("capsfilter");
        private Caps FilterCaps = Caps.FromString("video/x-h264");
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

        // Test Server: 
        // gst-launch-1.0 videotestsrc ! openh264enc ! h264parse ! rtph264pay ! udpsink host=127.0.0.1 port=5555

        // Client stream with recording and showing:
        // gst-launch-1.0 -vvv -e udpsrc caps=\"application/x-rtp, media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=5555 ! 
        // rtph264depay ! h264parse ! tee name=videoTee videoTee. ! queue ! mp4mux ! filesink location=test.mp4 videoTee. ! queue ! avdec_h264 ! d3dvideosink

        /// <summary>
        /// A window that is separate from the main UI and plays a GStreamer RTP feed.
        /// </summary>
        /// <param name="Port"> The port that the RTP stream is sending data to. </param>
        /// <param name="BufferSizeMs"> The time in milliseconds of buffering of the feed. </param>
        public RTPVideoWindow(int Port, string StreamName, string RecordingPath, int BufferSizeMs = 200)
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

            // Allow forwarding of EOS to all elements in the pipeline for shutdown
            Pipeline["message-forward"] = true;
            UDP["port"] = Port;
            UDP["caps"] = Caps;
            JitterBuffer["latency"] = BufferSizeMs;

            FileSink["location"] = RecordingPath + GetRecordingFilename();
            Filter["caps"] = FilterCaps;

            Pipeline.Add(UDP, JitterBuffer, Depay, Parse, Tee, Q1, Filter, Mux, FileSink, Q2, Dec, VideoSink);
            if(!Element.Link(UDP, JitterBuffer, Depay, Parse, Tee)) { Console.WriteLine("Failed To Link UDP, JitterBufferm, Depay, Parser, Tee"); }
            //if(!Parse.Link(Tee)) { Console.WriteLine("Failed To Link Parser to Tee"); }
            if(!Element.Link(Q1, Filter, Mux, FileSink)) { Console.WriteLine("Failed To Link Queue1, Mux, FileSink"); }
            if(!Element.Link(Q2, Dec, VideoSink)) { Console.WriteLine("Failed To Link Queue2, Decoder, VideoSink"); }

            PadTemplate TeeSrcPadTemplate = Tee.GetPadTemplate("src_%u");
            Pad TeeQ1Pad = Tee.RequestPad(TeeSrcPadTemplate);
            Pad Q1Pad = Q1.GetStaticPad("sink");

            Pad TeeQ2Pad = Tee.RequestPad(TeeSrcPadTemplate);
            Pad Q2Pad = Q2.GetStaticPad("sink");

            if (TeeQ1Pad.Link(Q1Pad) != PadLinkReturn.Ok) { Console.WriteLine("Failed To Link Tee to Queue1"); }
            if (TeeQ2Pad.Link(Q2Pad) != PadLinkReturn.Ok) { Console.WriteLine("Failed To Link Tee to Queue2"); }

            Q1Pad.Unref();
            Q2Pad.Unref();

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

        private string GetRecordingFilename()
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
