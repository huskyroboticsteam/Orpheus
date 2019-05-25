#include <cstdio>
#include <cstdlib>
#include <csignal>
#include <cstring>
#include <gst/gst.h>
#include <unistd.h>

#ifdef DEBUG
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
#endif

GstDeviceMonitor *device_monitor(void) 
{
    GstDeviceMonitor *monitor;
    GstBus *bus;
    GstCaps *caps;

    // starts the monitor for the devices 
    monitor = gst_device_monitor_new();

    // starts the bus for the monitor
    bus = gst_device_monitor_get_bus(monitor);
    gst_object_unref(bus);

    // adds a filter to scan for only video devices
    caps = gst_caps_new_empty_simple("video/x-raw");
    gst_device_monitor_add_filter(monitor, "Video/Source", caps);
    gst_caps_unref(caps);

    gst_device_monitor_start(monitor);

    return monitor;
}

int main(int argc, char *argv[]) 
{
    GstDeviceMonitor *monitor;
    GList *dev;
    GMainLoop *loop;
    gst_init(&argc, &argv);

    // create the monitor
    monitor = device_monitor();
    dev = gst_device_monitor_get_devices(monitor);
  
    // loop for the lists
    GList *cur = g_list_first(dev);
    while(cur != NULL) 
    {
        GstDevice * devi = (GstDevice *) cur->data;
        GstStructure * type = gst_device_get_properties(devi);

#ifdef DEBUG
        structure_fields(type);
#endif
        const char *name = gst_device_get_display_name(devi);
        const char *path = gst_structure_get_string(type, "device.path");
        g_print("Device-Scanner > Starting process for %s camera at %s\n", name, path);
        int mypid = fork();
        if(mypid == 0)
        {
            int ret;
            if (strcmp(name, "ZED") == 0) 
            {
                ret = execl("../zed_depth/zed_depth", argv[0]);
            }
            else
            {
                ret = execl("./server-wrapper", argv[0], name, path, NULL);
            }
            return ret; // Shouldn't reach this line
        }
        cur = g_list_next(cur);
    }

    loop = g_main_loop_new(NULL, FALSE);
    g_main_loop_run(loop);
    g_main_loop_unref(loop);

    return 0;
}
