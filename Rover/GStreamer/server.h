#ifndef SERVER_H_   /* Include guard */
#define SERVER_H_

#include <gst/gst.h>

struct g_server_data
{
    int argc;
    const char *argv[8];
};

int start_server (int argc, char *argv[]);

#endif // SERVER_H_
