#ifndef SERVER_H_   /* Include guard */
#define SERVER_H_

struct g_server_data
{
    int argc;
    const char *argv[5];

    g_server_data() : argc(5) {}
};

int start_server (int argc, char *argv[]);
void *g_start_server(void *data);

#endif // SERVER_H_
