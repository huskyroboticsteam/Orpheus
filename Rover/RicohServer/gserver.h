#pragma once

//#include <gst/gstprotection.h>
#include <gst/gst.h>
// struct for the elements used by gstreamer
typedef struct g_element_st 
{
  gchar *name, *cl;
  GstStructure *str;
  GstElement *pipeline, *source, *depay, *encoder;
  GstElement *parse, *sink, *s_cap, *e_cap, *scale;
  GstElement *scale_cap, *video_rate, *v_cap;

} *g_element;

/*void structure_fields(const GstStructure *);
gboolean bus_func(GstBus *, GstMessage *, gpointer);
GstDeviceMonitor *device_monitor(void);
void sigintHandler(int);
void stream_start(GList *, gchar *, gint);*/ 

