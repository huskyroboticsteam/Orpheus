#ifndef SERVER_H_   /* Include guard */
#define SERVER_H_

#include <gst/gst.h>

struct g_server_data
{
    int argc;
    const char *argv[8];
};

typedef struct
{
  gboolean white;
  GstClockTime timestamp;
} MyContext;

int start_server (int argc, char *argv[], void (*fnc)(GstElement *, guint, MyContext *));

#endif // SERVER_H_
