#ifndef GSERVER_H
#define GSERVER_H

#include <gst/gstprotection.h>
#include <gst/gst.h>

typedef struct device_st {
  const gchar *name;
  const gchar *path;
} *device_stat;

void structure_fields(const GstStructure *);
gboolean bus_func(GstBus *, GstMessage *, gpointer);
GstDeviceMonitor * device_monitor();
void sigintHandler(int);

#endif 
