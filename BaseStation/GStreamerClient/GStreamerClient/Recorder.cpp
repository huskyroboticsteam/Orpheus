#include <cstdio> 
#include <gst/gst.h>
#include <gst/app/gstappsink.h>
#include <cstring>
#include <Windows.h>

GstElement *pipeline;

typedef struct _CustomData {
	gboolean is_live;
	GstElement *pipeline;
	GMainLoop *loop;
} CustomData;

static void cb_message(GstBus *bus, GstMessage *msg, CustomData *data) {
	switch (GST_MESSAGE_TYPE(msg)) {
	case GST_MESSAGE_ERROR: {
		GError *err;
		gchar *debug;

		gst_message_parse_error(msg, &err, &debug);
		g_print("Error: %s\n", err->message);
		g_error_free(err);
		g_free(debug);

		gst_element_set_state(data->pipeline, GST_STATE_READY);
		g_main_loop_quit(data->loop);
		break;
	}
	case GST_MESSAGE_EOS:
		/* end-of-stream */
		printf("EOS\n");
		gst_element_set_state(data->pipeline, GST_STATE_READY);
		g_main_loop_quit(data->loop);
		break;
	case GST_MESSAGE_BUFFERING: {
		gint percent = 0;

		/* If the stream is live, we do not care about buffering. */
		if (data->is_live) break;

		gst_message_parse_buffering(msg, &percent);
		g_print("Buffering (%3d%%)\r", percent);
		/* Wait until buffering is complete before start/resume playing */
		if (percent < 100)
			gst_element_set_state(data->pipeline, GST_STATE_PAUSED);
		else
			gst_element_set_state(data->pipeline, GST_STATE_PLAYING);
		break;
	}
	case GST_MESSAGE_CLOCK_LOST:
		/* Get a new clock */
		gst_element_set_state(data->pipeline, GST_STATE_PAUSED);
		gst_element_set_state(data->pipeline, GST_STATE_PLAYING);
		break;
	default:
		/* Unhandled message */
		break;
	}
}


int sigintHandler(DWORD sig) {

	switch (sig) {
	case CTRL_C_EVENT:
		gst_element_send_event(pipeline, gst_event_new_eos());
		break;
	}
	return TRUE;
}


int main(int argc, char *argv[])
{
	GstBus *bus;
	GstStateChangeReturn ret;
	GMainLoop *main_loop;
	CustomData data = {};
	GstElement *udp, *depay, *parse, *mux, *filesink;
	GstCaps *caps;

	/* Set up callback to catch Ctrl-C event in console */
	SetConsoleCtrlHandler((PHANDLER_ROUTINE)sigintHandler, TRUE);	

	/* Initialize GStreamer */
	gst_init(&argc, &argv);

	/* Build the pipeline */
	//pipeline = gst_parse_launch("udpsrc caps=\"application/x-rtp, media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=5555 ! rtph264depay ! h264parse ! mp4mux ! filesink location=test.mp4", NULL);
	
	/* Generate pipeline elements */
	pipeline = gst_pipeline_new("pipeline");
	udp = gst_element_factory_make("udpsrc", "udp");
	depay = gst_element_factory_make("rtph264depay", "depay");
	parse = gst_element_factory_make("h264parse", "parse");
	mux = gst_element_factory_make("mp4mux", "mux");
	filesink = gst_element_factory_make("filesink", "filesink");

	/* Set properties of the pipeline elements 
	   All g_object_set calls end with NULL to indicate end of parameters */
	g_object_set(G_OBJECT(udp), "port", 5555, NULL);

	caps = gst_caps_new_simple("application/x-rtp", "media", G_TYPE_STRING, "video", 
		"width", G_TYPE_INT, 1280, "height", G_TYPE_INT, 720, "clock-rate", G_TYPE_INT, 90000, 
		"encoding-name", G_TYPE_STRING, "H264", NULL);
	g_object_set(G_OBJECT(udp), "caps", caps, NULL);

	g_object_set(G_OBJECT(filesink), "location", "test.mp4", NULL);
	
	/* Add created elements to the pipeline in the given order */
	gst_bin_add_many(GST_BIN(pipeline), udp, depay, parse, mux, filesink, NULL);

	/* Link the elements together in order and make sure they actually fit together */
	if (gst_element_link_many(udp, depay, parse, mux, filesink, NULL) != TRUE)
	{
		return -1;
	}

	/* Force pipeline to forward EOS to all elements */
	g_object_set(GST_BIN(pipeline), "message-forward", TRUE, NULL);

	/* Build the pipeline */
	bus = gst_element_get_bus(pipeline);

	/* Start playing */
	ret = gst_element_set_state(pipeline, GST_STATE_PLAYING);
	if (ret == GST_STATE_CHANGE_FAILURE) {
		g_printerr("Unable to set the pipeline to the playing state.\n");
		gst_object_unref(pipeline);
		return -1;
	}
	else if (ret == GST_STATE_CHANGE_NO_PREROLL) {
		data.is_live = TRUE;
	}

	main_loop = g_main_loop_new(NULL, FALSE);
	data.loop = main_loop;
	data.pipeline = pipeline;

	gst_bus_add_signal_watch(bus);
	g_signal_connect(bus, "message", G_CALLBACK(cb_message), &data);

	g_main_loop_run(main_loop);

	/* Free resources */
	g_main_loop_unref(main_loop);
	gst_object_unref(bus);
	gst_element_set_state(pipeline, GST_STATE_NULL);
	gst_object_unref(pipeline);
	return 0;
}
