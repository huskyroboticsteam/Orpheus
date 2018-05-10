#include <stdio.h>
#include <stdlib.h>
#include <signal.h>
#include "gserver.h"
#include <gst/gst.h>
#include <unordered_map>
#include <thread>
#include <vector> 
#include <chrono>

GMainLoop *loop;
std::vector<std::thread> threads;

void structure_fields(const GstStructure *device) 
{
  gint n_fields = gst_structure_n_fields(device);
  for (int i = 0; i < n_fields; i++) 
  {
    const gchar *name;
    name = gst_structure_nth_field_name(device, i);
    g_print("%d field name: %s\n", i, name);
  }
}

gboolean bus_func (GstBus *bus, GstMessage *message, gpointer user_data)
{
   GstDevice *devi;
   gchar *name;

   switch (GST_MESSAGE_TYPE (message)) 
   {
     case GST_MESSAGE_DEVICE_ADDED:
       GstStructure *type;

       gst_message_parse_device_added (message, &devi);
       name = gst_device_get_display_name (devi);
       type = gst_device_get_properties(devi);
       g_print("Device added: %s\n", name);
       g_free (name);
       g_print("Device path: %s\n", gst_structure_get_string(type, "device.path"));
       structure_fields(type);
       gst_structure_free(type);
       gst_object_unref (devi);
       break;
     case GST_MESSAGE_DEVICE_REMOVED:
       gst_message_parse_device_removed (message, &devi);
       name = gst_device_get_display_name (devi);
       g_print("Device removed: %s\n", name);
       g_free (name);
       gst_object_unref (devi);
       break;
     default:
       break;
   }

   return G_SOURCE_CONTINUE;
}

// v4l2src device="/dev/video0" ! 
// "video/x-raw, format=(string)I420, width=(int)1280, height=(int)720" ! 
// omxh264enc ! 
// 'video/x-h264, stream-format=(string)byte-stream' ! 
// h264parse ! 
// rtph264pay ! 
// udpsink host=192.168.0.5 port=5555
GstElement *start_device(GstDevice *dev, const char *client) 
{
  GstStructure *str;
  gchar *name, *cl;
  GstElement *pipeline, *source, *depay, *encoder, *parse, *sink;
  GstElement *s_cap, *e_cap, *scale, *scale_cap;
  GstCaps *s_cap_unapplied, *e_cap_unapplied, *scale_cap_unapplied;

  name = gst_device_get_display_name(dev);
  cl = gst_device_get_device_class(dev);
  str = gst_device_get_properties(dev);
  printf("device name: %s\n", name);
  printf("device class: %s\n", cl);
  g_print("device path: %s\n", gst_structure_get_string(str, "device.path")); 
  
  // setting elements of pipeline
  pipeline = gst_pipeline_new(gst_device_get_display_name(dev));
  source = gst_element_factory_make("v4l2src", NULL);
  s_cap = gst_element_factory_make("capsfilter", NULL);
  e_cap = gst_element_factory_make("capsfilter", NULL);
  depay = gst_element_factory_make("rtph264pay", NULL);
  encoder = gst_element_factory_make("omxh264enc", NULL);
  parse = gst_element_factory_make("h264parse", NULL);
  sink = gst_element_factory_make("udpsink", NULL); 
  scale = gst_element_factory_make("videoscale", NULL);
  scale_cap = gst_element_factory_make("capsfilter", NULL);

  // options for the source
  g_object_set(G_OBJECT(source), "device", gst_structure_get_string(str, "device.path")
                                                                                , NULL);

  // camera settings: (orignal_x_res, orignal_y_res, port, scaled_x_res, scaled_y_res)
  std::unordered_map<std::string, std::tuple<gint, gint, gint, gint, gint> > table;
  table["RICOH THETA S"] = std::make_tuple(1280, 720, 3333, 640, 360);
  table["ZED"] = std::make_tuple(3840, 1080, 5555, 960, 270);
  table["USB 2.0 Camera"] = std::make_tuple(1280, 720, 7777, 640, 360);
 
  
  s_cap_unapplied = gst_caps_new_simple("video/x-raw", "format", G_TYPE_STRING, "I420",
                              "width", G_TYPE_INT, std::get<0>(table[name]), "height", 
                               G_TYPE_INT, std::get<1>(table[name]), NULL);
  g_object_set(G_OBJECT(s_cap), "caps", s_cap_unapplied, NULL);

  // options for the encoder
  e_cap_unapplied = gst_caps_new_simple("video/x-h264", "stream-format", 
                                     G_TYPE_STRING, "byte-stream", NULL);
  g_object_set(G_OBJECT(e_cap), "caps", e_cap_unapplied, NULL);

  // options for the videoscalar
  scale_cap_unapplied = gst_caps_new_simple("video/x-raw", "format", G_TYPE_STRING,"I420", 
                                    "width", G_TYPE_INT, std::get<3>(table[name]), 
                                    "height", G_TYPE_INT, std::get<4>(table[name]), NULL);
 
  g_object_set(G_OBJECT(scale_cap), "caps", scale_cap_unapplied, NULL);

  // options for the sink
  g_object_set(G_OBJECT(sink), "host", client, "port", std::get<2>(table[name]), NULL);
  
  // there is no reason this should fail other than the programmer not making something correctly
  if (!pipeline || !source || !s_cap || !e_cap || !depay || !encoder || !parse || !sink || !scale || !scale_cap) 
  { 
    g_printerr ("One element could not be created. SAD! \n");
    exit(-1);
  } 
  
  // add elements and linking them
  gst_bin_add_many(GST_BIN(pipeline), source, s_cap, e_cap, depay, 
                    encoder, parse, sink, scale, scale_cap, NULL);
 
  if (!gst_element_link_many(source, s_cap, scale, scale_cap, encoder, 
                                     e_cap, parse, depay, sink, NULL)) 
  {
    g_printerr ("unable to link the elments to the pipeline. SAD! \n");
    exit(-1);
  } 

  return pipeline;
}

GstDeviceMonitor *device_monitor(void) 
{
  GstDeviceMonitor *monitor;
  GstBus *bus;
  GstCaps *caps;

  // starts the monitor for the devices 
  monitor = gst_device_monitor_new ();

  // starts the bus for the monitor
  bus = gst_device_monitor_get_bus (monitor);
  gst_object_unref (bus);

  // adds a filter for the devices seen
  caps = gst_caps_new_empty_simple ("video/x-raw");
  gst_device_monitor_add_filter (monitor, "Video/Source", caps);
  gst_caps_unref (caps);

  gst_device_monitor_start (monitor);

  return monitor;
}

// creates the pipelines and uses them in different threads
void stream_start(GList *cur, gchar *ip, gint stream_dup_num) 
{
  GstElement *pipe;
  int retries = 0;
  pipe = start_device((GstDevice*) cur->data, ip);
  gchar *name = gst_device_get_display_name((GstDevice *) cur->data);
 
  // try and set the state of the camera pipeline, retry 5 times exit if it can't 
  while (gst_element_set_state(pipe, GST_STATE_PLAYING) == GST_STATE_CHANGE_FAILURE)
  {
    printf("oh no did not start %s, going to retry \n", name);
    retries ++;
    if (retries >= 5) 
    {
      printf("this %s isn't going to open, exiting!\n", name); 
      exit(-1);
    }
    std::this_thread::sleep_for(std::chrono::nanoseconds(retries * 100));
  }
  printf("%s stream started\n", name);
}

/*void sigintHandler(int signum) 
{
  
  exit(-1);

}*/

int main(int argc, char *argv[]) 
{
  GstDeviceMonitor *monitor;
  GList *dev;
  gint stream_duplication_number = 0;
  char *ip;
  gst_init(&argc, &argv);

  if (argc > 1)
  {
    ip = argv[1];
  }
  else if (argc > 2) 
  {
    ip = argv[1];
    stream_duplication_number = atoi(argv[2]);
  }
  else 
  {
    ip = (char*)"192.168.0.5";
  }
  // signal for closing
  // signal(SIGINT, sigintHandler);

  // set the monitor
  monitor = device_monitor();
  dev = gst_device_monitor_get_devices(monitor);
  
  // loop for the lists
  GList *cur = g_list_first(dev);

  while (cur != NULL) 
  {
    threads.push_back(std::thread(stream_start, cur, ip, stream_duplication_number));
    cur = g_list_next(cur);
  }
  for (std::thread &thread : threads) 
  {
    thread.join();
  }
 
  loop = g_main_loop_new(NULL, FALSE);
  g_main_loop_run(loop);
  g_main_loop_unref(loop);
  return 0;
}
