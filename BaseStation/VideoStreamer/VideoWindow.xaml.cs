using System;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading;
using Gst;
using Gst.Video;
using System.Windows.Interop;

namespace VideoStreamer
{
    /// <summary>
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {
        public VideoWindow()
        {
            Pipeline pipeline = new Pipeline();

            Element udp = ElementFactory.Make("udpsrc");
            Element jitterbuffer = ElementFactory.Make("rtpjitterbuffer");
            Element depay = ElementFactory.Make("rtph264depay");
            Element parse = ElementFactory.Make("h264parse");
            Element dec = ElementFactory.Make("openh264dec");
            Element sink = ElementFactory.Make("d3dvideosink");
            Caps caps = Caps.FromString("application/x-rtp, media=video, clock-rate=90000, encoding-name=H264");

            udp["port"] = 5555;
            udp["caps"] = caps;

            pipeline.Add(udp, jitterbuffer, depay, parse, dec, sink);
            Element.Link(udp, jitterbuffer, depay, parse, dec, sink);

            pipeline.SetState(State.Null);

            pipeline.Bus.EnableSyncMessageEmission();
            pipeline.Bus.AddSignalWatch();
            pipeline.Bus.SyncMessage += SyncMessageHandler;

            StateChangeReturn s = pipeline.SetState(State.Playing);

            InitializeComponent();
        }

        private void SyncMessageHandler(object bus, SyncMessageArgs args)
        {
            Message msg = args.Message;

            if (!Gst.Video.Global.IsVideoOverlayPrepareWindowHandleMessage(msg))
                return;

            Element src = msg.Src as Element;
            if (src == null)
                return;

            try
            {
                src["force-aspect-ratio"] = true;
            }

            catch (PropertyNotFoundException) { }
            Element overlay = null;
            if (src is Bin)
                overlay = ((Bin)src).GetByInterface(VideoOverlayAdapter.GType);

            VideoOverlayAdapter adapter = new VideoOverlayAdapter(overlay.Handle);

            Window window = GetWindow(this);
            var wih = new WindowInteropHelper(window);
            adapter.WindowHandle = wih.Handle;

            adapter.HandleEvents(true);
        }
    }
}
