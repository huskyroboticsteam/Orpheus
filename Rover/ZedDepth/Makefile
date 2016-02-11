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

zed_depth_g: $(HEADERS) $(DEPTH).cpp server
	$(CPP) -O0 $(DEPTH).cpp -o $(DEPTH) server.o $(GSFLAGS) $(CVFLAGS) $(CFLAGS) $(INCLUDES) $(LIBS) -g
	
zed_depth_debug: $(HEADERS) $(DEPTH).cpp server
	$(CPP) -ggdb -D DEBUG $(DEPTH).cpp -o $(DEPTH) server.o $(GSFLAGS) $(CVFLAGS) $(CFLAGS) $(INCLUDES) $(LIBS)

zed_depth-only: zed_depth.cpp zed_depth.h
	g++ -c zed_depth.cpp -o zed_depth.o `pkg-config --cflags --libs gstreamer-1.0` `pkg-config --cflags --libs opencv4` -I/usr/local/zed/include -I/usr/local/cuda/include -I../GStreamer -std=c++14 -ggdb -O0


clean:
	rm $(DEPTH)
	rm *.o
