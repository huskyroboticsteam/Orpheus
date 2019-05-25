using Gst;
using Gst.Video;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace HuskyRobotics.UI.VideoStreamer
{
    /// <summary>
    /// Interaction logic for RTSPVideoWindow.xaml
    /// </summary>
    /// 
    // gst-launch-1.0 rtspsrc -v location = rtsp://192.168.0.42 user-id=admin user-pw=1234 latency=0 ! rtpjpegdepay ! jpegdec ! d3dvideosink
    public partial class RTSPVideoWindow : Window, VideoWindow
    {
        public PipelineElements runningPipeline;
        public PipelineElements pendingPipeline;

        double bytesRecvSpeed = 0;
        long lastBytesRecv;

        private string IP;
        private int Port;
        private string URI;
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

        public struct PipelineElements
        {
            public Pipeline pipeline;
            public VideoOverlayAdapter overlay;
            public Bin RTSP;
            public Element Depay;
            public Element VideoSink;
        }

        public RTSPVideoWindow(string StreamName, string IP, int Port, string URI, string RecordingPath, int BufferSizeMs = 200)
        {
            DataContext = this;
            this.IP = IP;
            this.Port = Port;
            this.URI = URI;
            this.StreamName = StreamName;
            this.BufferSizeMs = BufferSizeMs;
            this.RecordingPath = RecordingPath;

            PipelineElements pipeline = CreatePipeline();
            // Clean up running pipeline
            runningPipeline = pipeline;
            pendingPipeline = new PipelineElements();

            Closing += OnCloseEvent;
            InitializeComponent();
        }

        private PipelineElements CreatePipeline()
        {
            PipelineElements pipe = new PipelineElements();
            pendingPipeline = pipe;
            
            pipe.pipeline = new Pipeline();
            pipe.RTSP = (Bin)ElementFactory.Make("rtspsrc");
            pipe.Depay = ElementFactory.Make("rtpvrawdepay");
            pipe.VideoSink = ElementFactory.Make("d3dvideosink");

            Window window = GetWindow(this);
            WindowInteropHelper wih = new WindowInteropHelper(this);
            wih.EnsureHandle(); // Generate Window Handle if current HWND is NULL (always occurs because background window)
            pipe.overlay = new VideoOverlayAdapter(pipe.VideoSink.Handle);
            pipe.overlay.WindowHandle = wih.Handle;

            pipe.pipeline["message-forward"] = true;
            pipe.RTSP["location"] = "rtsp://" + IP + ":" + Port + "/" + URI;
            pipe.RTSP["latency"] = BufferSizeMs;

            pipe.pipeline.Add(pipe.RTSP, pipe.Depay, pipe.VideoSink);
            pipe.RTSP.PadAdded += RTSPPadAdded;
            pipe.RTSP.Link(pipe.Depay);
            pipe.Depay.Link(pipe.VideoSink);

            pipe.pipeline.SetState(State.Null);

            return pipe;
        }

        // Called after pipeline state is set to playing
        private void RTSPPadAdded(object o, PadAddedArgs args)
        {
            Pad Sink = runningPipeline.Depay.GetStaticPad("sink");
            args.NewPad.Link(Sink);
        }

        /// <summary>
        /// Window (this) must be visible before this method is called.
        /// </summary>
        public void StartStream()
        {
            StateChangeReturn s = runningPipeline.pipeline.SetState(State.Playing);
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
            runningPipeline.pipeline.SendEvent(Event.NewEos());
            //Thread.Sleep(1000); ADD BACK WHEN ADDING RECORDING

            // Cleanup the unmanaged class objects
            runningPipeline.pipeline.SetState(State.Null);
            runningPipeline.pipeline.Unref();
        }
    }
}
