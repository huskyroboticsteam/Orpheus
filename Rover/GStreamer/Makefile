CPP= g++
CFLAGS= -g -Wall -Wpedantic -std=c++14 
GSFLAGS= `pkg-config --cflags --libs gstreamer-1.0` -lgstrtspserver-1.0
HEADERS= 
OBJS= ricoh.o
DEV= device-scanner
SERVER= server

all: $(DEV) $(SERVER)

server: $(HEADERS) $(SERVER).cpp
	$(CPP) $(SERVER).cpp -o $(SERVER) $(GSFLAGS) $(CFLAGS)
	

device-scanner: $(HEADERS) $(DEV).cpp
	$(CPP) $(DEV).cpp -o $(DEV) $(GSFLAGS) $(CFLAGS)

clean:
	rm $(DEV) $(SERVER)
