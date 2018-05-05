CFLAGS= -g -Wall -Wpedantic -std=c++14 
GSFLAGS= `pkg-config --cflags --libs gstreamer-1.0`
HEADERS= gserver.h

all: g_server

g_server: $(HEADERS)
	g++ $(CFLAGS) gserver.cpp -o gserver $(GSFLAGS)

clean:
	rm gserver
