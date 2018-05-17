CFLAGS= -g -Wall -Wpedantic -std=c++14 
GSFLAGS= `pkg-config --cflags --libs gstreamer-1.0`
RDIR = ricoh #where the ricoh specific camera stuff is
HEADERS= gserver.h $(RDIR)/ricoh.h

all: g_server

g_server: $(HEADERS)
	g++ $(CFLAGS) gserver.cpp -o gserver $(GSFLAGS)

clean:
	rm gserver
