#pragma once

#include <gst/gstprotection.h>
#include <gst/gst.h>

void structure_fields(const GstStructure *);
gboolean bus_func(GstBus *, GstMessage *, gpointer);
GstDeviceMonitor *device_monitor(void);
void sigintHandler(int);
void stream_start(GList *, gchar *, gint); 

