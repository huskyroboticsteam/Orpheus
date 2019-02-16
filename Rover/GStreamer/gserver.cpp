#include <stdio.h>
#include <stdlib.h>
#include <signal.h>
#include <gst/gst.h>

void structure_fields(const GstStructure *device) 
{
  gint n_fields = gst_structure_n_fields(device);
  for (int i = 0; i < n_fields; i++) 
  {
    const gchar *name;
    name = gst_structure_nth_field_name(device, i);
    g_print("%s:\n%s\n", name, gst_structure_get_string(device, name));
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

int main(int argc, char *argv[]) 
{
  g_print("Start\n");
  GstDeviceMonitor *monitor;
  GList *dev;
  char *ip;
  gst_init(&argc, &argv);

  // set the monitor
  g_print("Device Monitor\n");
  monitor = device_monitor();
  dev = gst_device_monitor_get_devices(monitor);
  
  // loop for the lists
  GList *cur = g_list_first(dev);
  while(cur != NULL) 
  {
    GstDevice * devi = (GstDevice *) cur->data;
    g_print("%s\n", gst_device_get_display_name(devi));
    GstStructure * type = gst_device_get_properties(devi);
    structure_fields(type);
    cur = g_list_next(cur);
  }

  return 0;
}
