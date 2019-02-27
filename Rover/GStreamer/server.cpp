#include <gst/gst.h>
#include <gst/rtsp-server/rtsp-server.h>
#include <unordered_map>
#include <string>
#include <vector>
#include <cstring>

using namespace std;

unordered_map<string, tuple<const char*, int, int, float, const char*>> table;

char *
construct_pipeline(char * devpath, const char * input_type, int width, int height, float scale)
{
  char * output = (char *) malloc(1024);
  char width_str[5];
  char height_str[5];
  char scaled_width_str[5];
  char scaled_height_str[5];

  int scaled_width = width * scale;
  int scaled_height = height * scale;

  sprintf(width_str, "%d", width);
  sprintf(height_str, "%d", height);
  sprintf(scaled_width_str, "%d", scaled_width);
  sprintf(scaled_height_str, "%d", scaled_height);

  if (strcmp(input_type, "I420") != 0)
  {
    sprintf(output, "v4l2src device=%s ! video/x-raw, format=%s, width=%s, height=%s ! videoconvert ! video/x-raw, format=I420, width=%s, height=%s ! videoscale ! video/x-raw, format=I420, width=%s, height=%s ! rtpvrawpay name=pay0 pt=96", devpath, input_type, width_str, height_str, width_str, height_str, scaled_width_str, scaled_height_str);
  }
  else
  {
    sprintf(output, "v4l2src device=%s ! video/x-raw, format=I420, width=%s, height=%s ! videoscale ! video/x-raw, format=I420, width=%s, height=%s ! rtpvrawpay name=pay0 pt=96", devpath, width_str, height_str, scaled_width_str, scaled_height_str);
  }


  return output;
}

void
setup_map()
{
  table["ZED"] = make_tuple("5556", 4416, 1242, 0.25f, "YUY2");
}

int
main (int argc, char *argv[])
{
  GMainLoop *loop;
  GstRTSPServer *server;
  GstRTSPMountPoints *mounts;
  GstRTSPMediaFactory *factory;
  char * devname;
  char * devpath;
  const char * port;
  char * pipeline;
  setup_map();
  gst_init(&argc, &argv);
  
  if (argc == 3) 
  {
    devname = argv[1];
    devpath = argv[2];
    if (table.count(devname) == 0)
    {
      g_print("%s@%s No pipeline found; provide pipeline argument\n", devname, devpath);
      return -1;
    }
    else
    {
      pipeline = construct_pipeline(devpath, get<4>(table.at(devname)), get<1>(table.at(devname)), get<2>(table.at(devname)), get<3>(table.at(devname)));
      port = get<0>(table.at(devname));
    }
  } 
  else if (argc == 5)
  { 
    devname = argv[1];
    devpath = argv[2];
    port = argv[3];
    pipeline = argv[4];
  }
  else
  {
    g_print("Usage ./serve <device name> <device path> <rtsp port> <pipeline>\n");
    return -1;
  }

  g_print("%s\n", pipeline);

  loop = g_main_loop_new (NULL, FALSE);

  /* create a server instance */
  server = gst_rtsp_server_new ();
  g_object_set (server, "service", port, NULL);

  /* get the mount points for this server, every server has a default object
   * that be used to map uri mount points to media factories */
  mounts = gst_rtsp_server_get_mount_points (server);

  /* make a media factory for a test stream. The default media factory can use
   * gst-launch syntax to create pipelines.
   * any launch line works as long as it contains elements named pay%d. Each
   * element with pay%d names will be a stream */
  factory = gst_rtsp_media_factory_new ();
  gst_rtsp_media_factory_set_launch (factory, pipeline);
  gst_rtsp_media_factory_set_shared (factory, TRUE);

  /* attach the test factory to the /test url */
  gst_rtsp_mount_points_add_factory (mounts, "/test", factory);

  /* don't need the ref to the mapper anymore */
  g_object_unref (mounts);

  /* attach the server to the default maincontext */
  gst_rtsp_server_attach (server, NULL);

  /* start serving */
  g_print ("%s@%s: stream ready at rtsp://127.0.0.1:%s/test\n", devname, devpath, port);
  g_main_loop_run (loop);
  g_main_loop_unref(loop);

  return 0;
}
