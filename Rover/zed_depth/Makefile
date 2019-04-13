CPP= g++
CFLAGS= -g -std=c++14 
GSFLAGS= `pkg-config --cflags --libs gstreamer-1.0` -lgstrtspserver-1.0
CVFLAGS= `pkg-config --cflags --libs opencv4`
INCLUDES= -I/usr/local/zed/include -I/usr/local/cuda/include -I../GStreamer
LIBS= /usr/local/zed/lib/*.so -march=armv8-a+simd
HEADERS= 
DEPTH= zed_depth

all: $(DEPTH)
	rm *.o

debug: $(DEPTH)_debug
	rm *.o

server: $(HEADERS) ../GStreamer/server.cpp
	$(CPP) -O3 -c ../GStreamer/server.cpp $(GSFLAGS) $(CFLAGS)

zed_depth: $(HEADERS) $(DEPTH).cpp server
	$(CPP) -O3 $(DEPTH).cpp -o $(DEPTH) server.o $(GSFLAGS) $(CVFLAGS) $(CFLAGS) $(INCLUDES) $(LIBS)
	
zed_depth_debug: $(HEADERS) $(DEPTH).cpp server
	$(CPP) -ggdb -D DEBUG $(DEPTH).cpp -o $(DEPTH) server.o $(GSFLAGS) $(CVFLAGS) $(CFLAGS) $(INCLUDES) $(LIBS)
	
clean:
	rm $(DEPTH)
