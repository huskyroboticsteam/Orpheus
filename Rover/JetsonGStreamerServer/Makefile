CPP= g++
CFLAGS= -g -Wall -Wpedantic -std=c++14 
GSFLAGS= `pkg-config --cflags --libs gstreamer-1.0`
CVFLAGS= -lopencv_core
HEADERS= gserver.h ricoh.h
OBJS= ricoh.o

all: ricoh g_server

#ricoh: ricoh.cpp $(HEADERS)
#	$(CPP) $(CFLAGS) -c ricoh.cpp $(CVFLAGS) $(GSFLAGS)

g_server: $(HEADERS) richo.cpp gserver.cpp
	$(CPP) -std=c++14 gserver.cpp -o gserver $(GSFLAGS) $(CVFLAGS)

clean:
	rm gserver *.o
