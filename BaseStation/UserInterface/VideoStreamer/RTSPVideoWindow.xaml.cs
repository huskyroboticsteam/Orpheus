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

        NetworkInterface networkInterface;

        double bytesRecvSpeed = 0;
        long lastBytesRecv;

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

        public struct PipelineElements
        {
            public Pipeline pipeline;
            public VideoOverlayAdapter overlay;
            public Bin RTSP;
            public Element Depay;
            public Element VideoSink;
        }

        public RTSPVideoWindow(int Port, string StreamName, string RecordingPath, int BufferSizeMs = 200)
        {
            DataContext = this;
            this.Port = Port;
            this.StreamName = StreamName;
            this.BufferSizeMs = BufferSizeMs;
            this.RecordingPath = RecordingPath;

            foreach (NetworkInterface currentNetworkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                Console.WriteLine(currentNetworkInterface.Description);
                if (currentNetworkInterface.Description == "Realtek USB GbE Family Controller") {
                    networkInterface = currentNetworkInterface;
                }
            }

            IPv4InterfaceStatistics interfaceStatistic = networkInterface.GetIPv4Statistics();
            lastBytesRecv = interfaceStatistic.BytesReceived;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(GetNetworkInBackground);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

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
            pipe.RTSP["location"] = "rtsp://" + StreamName;
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
            Thread.Sleep(1000);

            // Cleanup the unmanaged class objects
            runningPipeline.pipeline.SetState(State.Null);
            runningPipeline.pipeline.Unref();
        }

        private void GetNetworkInBackground(object sender, EventArgs e)
        {
            IPv4InterfaceStatistics interfaceStatistic = networkInterface.GetIPv4Statistics();
            long bytesRecv = interfaceStatistic.BytesReceived;
            bytesRecvSpeed = (bytesRecv - lastBytesRecv) * 8 / Math.Pow(1024, 2);
            Console.WriteLine("Receiving data at: " + string.Format("{0:0.00}", bytesRecvSpeed) + " Mbps" + " over " + networkInterface.Description);
            lastBytesRecv = bytesRecv;
        }
    }
}
