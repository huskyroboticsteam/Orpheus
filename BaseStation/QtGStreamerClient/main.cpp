#include <glib.h>
#include <gst/gst.h>
#include <gst/video/videooverlay.h>

#include <QApplication>
#include <QTimer>
#include <QWidget>
#include <QObject>

#include "mainwindow.h"

GstElement *pipeline;
GstElement *udp, *jitter, *depay, *parse, *dec, *sink;
GstCaps *caps;

static void
setup_pipeline()
{
    /* prepare the pipeline */
    pipeline = gst_pipeline_new("pipeline");
    udp = gst_element_factory_make("udpsrc", "udp");
    jitter = gst_element_factory_make("rtpjitterbuffer", "jitter");
    depay = gst_element_factory_make("rtph264depay", "depay");
    parse = gst_element_factory_make("h264parse", "parse");
    dec = gst_element_factory_make("openh264dec", "dec");
    sink = gst_element_factory_make("d3dvideosink", "sink");

    /* Set properties of the pipeline elements
       All g_object_set calls end with NULL to indicate end of parameters */
    g_object_set(G_OBJECT(udp), "port", 5555, NULL);

    caps = gst_caps_new_simple("application/x-rtp", "media", G_TYPE_STRING, "video",
        "width", G_TYPE_INT, 1280, "height", G_TYPE_INT, 720, "clock-rate", G_TYPE_INT, 90000,
        "encoding-name", G_TYPE_STRING, "H264", NULL);
    g_object_set(G_OBJECT(udp), "caps", caps, NULL);

    /* Add created elements to the pipeline in the given order */
    gst_bin_add_many(GST_BIN(pipeline), udp, jitter, depay, parse, dec, sink, NULL);

    /* Link the elements together in order and make sure they actually fit together */
    if (gst_element_link_many(udp, jitter, depay, parse, dec, sink, NULL) != 1) {
        printf("%p %p %p %p %p\n", udp, depay, parse, dec, sink);
        exit(-1);
    }
}

int main(int argc, char *argv[])
{
//    gst_init (&argc, &argv);
    QApplication app(argc, argv);

    // Server: gst-launch-1.0 videotestsrc ! "video/x-raw, format=(string)I420, width=(int)1280, height=(int)720" ! videoscale ! 'video/x-raw, width=640, height=340' ! openh264enc ! h264parse ! rtph264pay ! udpsink host=127.0.0.1 port=5555
    // Client: gst-launch-1.0 -vvv -e udpsrc caps=\"application/x-rtp, media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=5555 ! rtpjitterbuffer ! rtph264depay ! h264parse ! openh264dec ! d3dvideosink
//    setup_pipeline();

    /* prepare the ui */
    MainWindow window;
    window.resize(1280, 720);
    window.setWindowTitle("GstVideoOverlay Qt");
    window.show();

//    WId xwinid = window.winId();
//    gst_video_overlay_set_window_handle (GST_VIDEO_OVERLAY (sink), xwinid);

    /* run the pipeline */
//    GstStateChangeReturn sret = gst_element_set_state (pipeline,
//      GST_STATE_PLAYING);
//    if (sret == GST_STATE_CHANGE_FAILURE) {
//    gst_element_set_state (pipeline, GST_STATE_NULL);
//    gst_object_unref (pipeline);
//    /* Exit application */
//    QTimer::singleShot(0, QApplication::activeWindow(), SLOT(quit()));
//    }

    int ret = app.exec();

    window.hide();
//    gst_element_set_state (pipeline, GST_STATE_NULL);
//    gst_object_unref (pipeline);

    return ret;
}
