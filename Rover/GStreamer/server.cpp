#include <gst/gst.h>
#include <gst/rtsp-server/rtsp-server.h>
#include <unordered_map>
#include <string>
#include <vector>
#include <cstring>

using namespace std;

unordered_map<string, tuple<const char*, int, int, vector<float>*, const char*>> table;

void
set_launch(GstRTSPServer * server, char * pipeline, const char * index)
{
  GstRTSPMountPoints *mounts;
  GstRTSPMediaFactory *factory;
 
  /* get the mount points for this server, every server has a default object
   * that be used to map uri mount points to media factories */
  mounts = gst_rtsp_server_get_mount_points (server);

  factory = gst_rtsp_media_factory_new ();
  gst_rtsp_media_factory_set_launch (factory, pipeline);
  gst_rtsp_media_factory_set_shared (factory, TRUE);

  /* attach the test factory to the /feed + i url */
  string attach = "feed";
  attach += index;
  gst_rtsp_mount_points_add_factory (mounts, attach.c_str(), factory);
 
  /* don't need the ref to the mapper anymore */
  g_object_unref (mounts); 
}

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
    sprintf(output, "intervideosrc ! video/x-raw, format=%s, width=%s, height=%s ! videoconvert ! video/x-raw, format=I420, width=%s, height=%s ! videoscale ! video/x-raw, format=I420, width=%s, height=%s ! rtpvrawpay name=pay0 pt=96", input_type, width_str, height_str, width_str, height_str, scaled_width_str, scaled_height_str);
  }
  else
  {
    sprintf(output, "intervideosrc ! video/x-raw, format=I420, width=%s, height=%s ! videoscale ! video/x-raw, format=I420, width=%s, height=%s ! rtpvrawpay name=pay0 pt=96", width_str, height_str, scaled_width_str, scaled_height_str);
  }


  return output;
}

void
setup_map()
{
  vector<float> * zedlist = new vector<float>;
  zedlist->push_back(0.25f);
  zedlist->push_back(0.1f);
  table["ZED"] = make_tuple("5556", 4416, 1242, zedlist, "YUY2");
  
}

int
main (int argc, char *argv[])
{
  GMainLoop *loop;
  GstElement *inputpipe;
  GstRTSPServer *server;
  char * devname;
  char * devpath;
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
      input += " ! intervideosink";
      GstElement * inputpipe = gst_parse_launch(input.c_str(), NULL);
      int ret = gst_element_set_state(inputpipe, GST_STATE_PLAYING);

      if (ret == GST_STATE_CHANGE_FAILURE)
      {
        exit(-1);
      }

      tuple<const char*, int, int, vector<float>*, const char*> item = table.at(devname);
      g_print("Hello");
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

  /* make a media factory for a test stream. The default media factory can use
   * gst-launch syntax to create pipelines.
   * any launch line works as long as it contains elements named pay%d. Each
   * element with pay%d names will be a stream */
  for (int i = 0; i < (int) pipelines.size(); i++)
  {
    set_launch(server, pipelines[i], to_string(i).c_str());  
    g_print ("%s@%s > stream ready at rtsp://127.0.0.1:%s/feed%d\n", devname, devpath, port, i);
  }

  /* attach the server to the default maincontext */
  gst_rtsp_server_attach (server, NULL);

  /* start serving */
  g_main_loop_run (loop);
  g_main_loop_unref(loop);

  return 0;
}
