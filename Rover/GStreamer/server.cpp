#include <gst/gst.h>
#include <gst/rtsp-server/rtsp-server.h>
#include <unordered_map>
#include <string>
#include <vector>
#include <cstring>

#include "server.h"

using namespace std;

unordered_map<string, tuple<const char*, int, int, vector<float>*, const char*>> table;

void
media_configure (GstRTSPMediaFactory * factory, GstRTSPMedia * media, gpointer user_data)
{
  GstElement *element, *appsrc;
  MyContext *ctx;

  element = gst_rtsp_media_get_element(media);
  appsrc = gst_bin_get_by_name_recurse_up (GST_BIN (element), "mysrc");

  gst_util_set_object_arg (G_OBJECT (appsrc), "format", "time");

  ctx = g_new0(MyContext, 1);
  ctx->white = FALSE;
  ctx->timestamp = 0;

  g_object_set_data_full (G_OBJECT (media), "my-extra-data", ctx, (GDestroyNotify) g_free);
  
  g_signal_connect (appsrc, "need-data", (GCallback) *(void (**) (GstElement *, guint, MyContext *)) user_data, ctx);

  gst_object_unref(appsrc);
  gst_object_unref(element);
}

char *
construct_pipeline(const char * devpath, const char * input_type, int width, int height, float scale)
{
    char * output = (char *) malloc(512);

    int scaled_width = width * scale;
    int scaled_height = height * scale;

    if (strcmp(input_type, "I420") != 0)
    {
        snprintf(output, 512, "intervideosrc ! video/x-raw, format=%s, width=%d, height=%d ! videoscale ! videoconvert ! video/x-raw, format=I420, width=%d, height=%d ! rtpvrawpay name=pay0 pt=96", input_type, width, height, scaled_width, scaled_height);
    }
    else
    {
        snprintf(output, 512, "intervideosrc ! video/x-raw, format=I420, width=%d, height=%d ! videoscale ! video/x-raw, format=I420, width=%d, height=%d ! rtpvrawpay name=pay0 pt=96", width, height, scaled_width, scaled_height);
    }


    return output;
}

void
setup_map()
{
    vector<float> * zedlist = new vector<float>;
    zedlist->push_back(0.25f);
    zedlist->push_back(0.1f);
    vector<float> * usb2list = new vector<float>;
    usb2list->push_back(0.25f);
    usb2list->push_back(0.1f);
    table["ZED"] = make_tuple("5556", 4416, 1242, zedlist, "YUY2");
    table["USB 2.0 Camera"] = make_tuple("5557", 1920, 1080, usb2list, "YUY2");  
}

int 
start_server (int argc, char *argv[], void (*fnc) (GstElement *, guint, MyContext *))
{
    GMainLoop *loop;
    GstRTSPServer *server;
    const char * devname;
    const char * devpath;
    const char * port;
    vector<char *> pipelines;
    setup_map();
    gst_init(&argc, &argv);

    if (argc == 3)  // Want to run a predefined camera which is at defined path
    {
        devname = argv[1];
        devpath = argv[2];
        if (table.count(devname) == 0)
        {
            g_print("%s@%s > No pipeline found; provide pipeline argument\n", devname, devpath);
            return -1;
        }
        else
        {
            string input = "v4l2src device=";
            input += devpath;
            input += " ! videorate ! video/x-raw, framerate=10/1 ! intervideosink";
            GstElement * inputpipe = gst_parse_launch(input.c_str(), NULL);
            int ret = gst_element_set_state(inputpipe, GST_STATE_PLAYING);

            if (ret == GST_STATE_CHANGE_FAILURE)
            {
                exit(-1);
            }
            else
            {
                g_print("%s@%s > Opened camera successfully\n", devname, devpath);
            }

            tuple<const char*, int, int, vector<float>*, const char*> item = table.at(devname);
            for (int i = 0; i < (int) get<3>(item)->size(); i++)
            {
                pipelines.push_back(construct_pipeline(devpath, get<4>(item), get<1>(item), get<2>(item), (*(get<3>(item)))[i]));
#ifdef DEBUG
                g_print("%s@%s > %s\n", devname, devpath, pipelines[i]);
#endif
            }
      
            port = get<0>(item);
        }
    } 
    else if (argc == 5) // Likely from command line where everything is defined as an argument
    { 
        devname = argv[1];
        devpath = argv[2];
        port = argv[3];
        pipelines.push_back(argv[4]);
    }
    else
    {
        g_print("Usage ./serve <device name> <device path> <rtsp port> <pipeline>\n");
        return -1;
    }

    loop = g_main_loop_new (NULL, FALSE);

    /* create a server instance */
    server = gst_rtsp_server_new ();
    g_object_set (server, "service", port, NULL);

    /* get the mount points for this server, every server has a default object
     * that be used to map uri mount points to media factories */
    GstRTSPMountPoints * mounts = gst_rtsp_server_get_mount_points (server);
    char attachment[32];

    for (unsigned short i = 0; i < (int) pipelines.size(); i++)
    {  
        GstRTSPMediaFactory * factory = gst_rtsp_media_factory_new ();
        gst_rtsp_media_factory_set_launch (factory, pipelines[i]);
        gst_rtsp_media_factory_set_shared (factory, TRUE);
        if (fnc != NULL) 
        {
            g_signal_connect (factory, "media-configure", (GCallback) media_configure, &fnc); 
        }
        snprintf(attachment, 10, "/feed%hu", i); 
        gst_rtsp_mount_points_add_factory (mounts, attachment, factory);
        g_print ("%s@%s: stream ready at rtsp://127.0.0.1:%s/feed%d\n", devname, devpath, port, i);
    }
  
    /* don't need the ref to the mapper anymore */
    g_object_unref (mounts);

    /* attach the server to the default maincontext */
    gst_rtsp_server_attach (server, NULL);

    /* start serving */
    g_main_loop_run (loop);
    g_main_loop_unref(loop);
    return 0;
}
