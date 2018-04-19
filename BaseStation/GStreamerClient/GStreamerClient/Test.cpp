//#include <string.h>
//#include <gst/gst.h>
//#include <signal.h>
//#include <stdlib.h>
//#include <stdio.h>
//#include <Windows.h>
//
////	udpsrc port=8554 caps="application/x-rtp, media=(string)video, clock-rate=(int)90000, width=(int)720, height=(int)480, encoding-name=(string)H264, payload=(int)96" ! 
////			rtpjitterbuffer name=rtpjitbuff ! rtph264depay ! 
////				tee name=t t. ! avdec_h264 ! appsink name=sink sync=false 
//// 							t. ! h264parse ! mp4mux ! filesink location=/home/rish/Desktop/recorded.mp4
//
////								/	queue ! avdec_h264 ! appsink
//// udpsrc ! capsfilter ! rtpjitterbuffer ! rtph264depay ! tee ! 
////								\	queue ! h264parse ! matroskamux ! filesink
//
//static GMainLoop *loop;
//static GstElement *pipeline, *src, *rtpjitterbuffer, *rtph264depay, *tee, *decoder, *appsink, *h264parser, *muxer, *filesink, *queue_record, *queue_display;
//static GstBus *bus;
//static GstPad *teepad;
//static gboolean recording = FALSE;
//static gint counter = 0;
//
//static void finalizeRecording();
//
//static gboolean
//message_cb(GstBus * bus, GstMessage * message, gpointer user_data)
//{
//	switch (GST_MESSAGE_TYPE(message)) {
//	case GST_MESSAGE_ERROR: {
//		GError *err = NULL;
//		gchar *name, *debug = NULL;
//
//		name = gst_object_get_path_string(message->src);
//		gst_message_parse_error(message, &err, &debug);
//
//		g_printerr("ERROR: from element %s: %s\n", name, err->message);
//		if (debug != NULL)
//			g_printerr("Additional debug info:\n%s\n", debug);
//
//		g_error_free(err);
//		g_free(debug);
//		g_free(name);
//
//		g_main_loop_quit(loop);
//		break;
//	}
//	case GST_MESSAGE_WARNING: {
//		GError *err = NULL;
//		gchar *name, *debug = NULL;
//
//		name = gst_object_get_path_string(message->src);
//		gst_message_parse_warning(message, &err, &debug);
//
//		g_printerr("ERROR: from element %s: %s\n", name, err->message);
//		if (debug != NULL)
//			g_printerr("Additional debug info:\n%s\n", debug);
//
//		g_error_free(err);
//		g_free(debug);
//		g_free(name);
//		break;
//	}
//	case GST_MESSAGE_EOS: {
//		g_print("Got EOS\n");
//		// g_main_loop_quit (loop);
//		// gst_element_set_state (pipeline, GST_STATE_NULL);
//		// g_main_loop_unref (loop);
//		// gst_object_unref (pipeline);
//		// exit(0);
//		break;
//	}
//	case GST_MESSAGE_ELEMENT: {
//		const GstStructure *s = gst_message_get_structure(message);
//
//		if (gst_structure_has_name(s, "GstBinForwarded")) {
//			GstMessage *forward_msg = NULL;
//
//			gst_structure_get(s, "message", GST_TYPE_MESSAGE, &forward_msg, NULL);
//			if (GST_MESSAGE_TYPE(forward_msg) == GST_MESSAGE_EOS) {
//				g_print("EOS from element %s\n", GST_OBJECT_NAME(GST_MESSAGE_SRC(forward_msg)));
//				finalizeRecording();
//			}
//			gst_message_unref(forward_msg);
//		}
//		break;
//	}
//	default:
//		break;
//	}
//
//	return TRUE;
//}
//
//static void finalizeRecording() {
//	gst_bin_remove(GST_BIN(pipeline), queue_record);
//	gst_bin_remove(GST_BIN(pipeline), h264parser);
//	gst_bin_remove(GST_BIN(pipeline), muxer);
//	gst_bin_remove(GST_BIN(pipeline), filesink);
//
//	gst_element_set_state(queue_record, GST_STATE_NULL);
//	gst_element_set_state(h264parser, GST_STATE_NULL);
//	gst_element_set_state(muxer, GST_STATE_NULL);
//	gst_element_set_state(filesink, GST_STATE_NULL);
//
//	gst_object_unref(queue_record);
//	gst_object_unref(h264parser);
//	gst_object_unref(muxer);
//	gst_object_unref(filesink);
//
//	gst_element_release_request_pad(tee, teepad);
//	gst_object_unref(teepad);
//
//	recording = FALSE;
//	g_print("Unlinked\n");
//}
//
//static GstPadProbeReturn unlink_cb(GstPad *pad, GstPadProbeInfo *info, gpointer user_data) {
//	g_print("Unlinking...");
//	GstPad *sinkpad;
//	sinkpad = gst_element_get_static_pad(queue_record, "sink");
//	gst_pad_unlink(teepad, sinkpad);
//	gst_object_unref(sinkpad);
//
//	gst_element_send_event(muxer, gst_event_new_eos());
//
//	return GST_PAD_PROBE_REMOVE;
//}
//
//void stopRecording() {
//	g_print("stopRecording\n");
//	gst_pad_add_probe(teepad, GST_PAD_PROBE_TYPE_IDLE, unlink_cb, NULL, (GDestroyNotify)g_free);
//}
//
//void startRecording() {
//	g_print("startRecording\n");
//	GstPad *sinkpad;
//	GstPadTemplate *templ;
//
//	templ = gst_element_class_get_pad_template(GST_ELEMENT_GET_CLASS(tee), "src_%u");
//	teepad = gst_element_request_pad(tee, templ, NULL, NULL);
//	queue_record = gst_element_factory_make("queue", "queue_record");
//	h264parser = gst_element_factory_make("h264parse", NULL);
//	muxer = gst_element_factory_make("matroskamux", NULL);
//	filesink = gst_element_factory_make("filesink", NULL);
//	char *file_name = (char*)malloc(100 * sizeof(char));
//	sprintf(file_name, "rec%d.mp4", counter++);
//	g_print("file created %s\n", file_name);
//	g_object_set(filesink, "location", file_name, NULL);
//	free(file_name);
//
//	gst_bin_add_many(GST_BIN(pipeline), queue_record, h264parser, muxer, filesink, NULL);
//	gst_element_link_many(queue_record, h264parser, muxer, filesink, NULL);
//
//	gst_element_sync_state_with_parent(queue_record);
//	gst_element_sync_state_with_parent(h264parser);
//	gst_element_sync_state_with_parent(muxer);
//	gst_element_sync_state_with_parent(filesink);
//
//	sinkpad = gst_element_get_static_pad(queue_record, "sink");
//	gst_pad_link(teepad, sinkpad);
//	gst_object_unref(sinkpad);
//
//	recording = TRUE;
//}
//
//int sigintHandler(int unused) {
//	g_print("You ctrl-c!\n");
//	if (recording)
//		stopRecording();
//	else
//		startRecording();
//
//	return TRUE;
//}
//
//int main(int argc, char *argv[])
//{
//	SetConsoleCtrlHandler((PHANDLER_ROUTINE)sigintHandler, TRUE);
//	gst_init(&argc, &argv);
//
//	pipeline = gst_pipeline_new(NULL);
//	src = gst_element_factory_make("udpsrc", NULL);
//	rtpjitterbuffer = gst_element_factory_make("rtpjitterbuffer", NULL);
//	rtph264depay = gst_element_factory_make("rtph264depay", NULL);
//	tee = gst_element_factory_make("tee", "tee");
//	queue_display = gst_element_factory_make("queue", "queue_display");
//	decoder = gst_element_factory_make("openh264dec", NULL);
//	appsink = gst_element_factory_make("autovideosink", "sink");
//
//	if (!pipeline || !src || !rtpjitterbuffer || !rtph264depay || !tee || !queue_display || !decoder || !appsink) {
//		printf("%p, %p, %p, %p, %p, %p, %p, %p\n", pipeline, src, rtpjitterbuffer, rtph264depay, tee, queue_display, decoder, appsink);
//		g_error("Failed to create elements");
//		return -1;
//	}
//
//	gst_bin_add_many(GST_BIN(pipeline), src, rtpjitterbuffer, rtph264depay, tee, queue_display, decoder, appsink, NULL);
//	if (!gst_element_link_many(src, rtpjitterbuffer, rtph264depay, tee, NULL)
//		|| !gst_element_link_many(tee, queue_display, decoder, appsink, NULL)) {
//		g_error("Failed to link elements");
//		return -2;
//	}
//
//	g_object_set(GST_BIN(pipeline), "message-forward", TRUE, NULL);
//	g_object_set(src, "port", 5555, NULL);
//	GstCaps *caps = gst_caps_from_string("application/x-rtp, media=(string)video, clock-rate=(int)90000, width=(int)1280, height=(int)720, encoding-name=(string)H264");
//	g_object_set(src, "caps", caps, NULL);
//
//	startRecording();
//	loop = g_main_loop_new(NULL, FALSE);
//
//	bus = gst_pipeline_get_bus(GST_PIPELINE(pipeline));
//	gst_bus_add_signal_watch(bus);
//	g_signal_connect(G_OBJECT(bus), "message", G_CALLBACK(message_cb), NULL);
//	gst_object_unref(GST_OBJECT(bus));
//
//	gst_element_set_state(pipeline, GST_STATE_PLAYING);
//
//	g_print("Starting loop\n");
//	g_main_loop_run(loop);
//
//	return 0;
//}