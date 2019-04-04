#ifndef SERVER_H_   /* Include guard */
#define SERVER_H_

#include <gst/gst.h>
#include <gst/rtsp-server/rtsp-server.h>
#include <unordered_map>
#include <string>
#include <vector>
#include <cstring>

int start_server (int argc, char *argv[]);

#endif // SERVER_H_
