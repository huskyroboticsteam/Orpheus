CPP= g++
CFLAGS= -g -Wall -Wpedantic -std=c++14 
GSFLAGS= `pkg-config --cflags --libs gstreamer-1.0` -lgstrtspserver-1.0
HEADERS= 
DEV= device-scanner
SERVER= server-wrapper

all: $(DEV) $(SERVER)
	rm *.o

debug: $(DEV)_debug $(SERVER)_debug
	rm *.o

server: $(HEADERS) server.cpp
	$(CPP) -O3 -c server.cpp $(GSFLAGS) $(CFLAGS)

server-wrapper: $(HEADERS) $(SERVER).cpp server
	$(CPP) -O3 $(SERVER).cpp -o $(SERVER) server.o $(GSFLAGS) $(CFLAGS)
	
device-scanner: $(HEADERS) $(DEV).cpp
	$(CPP) -O3 $(DEV).cpp -o $(DEV) $(GSFLAGS) $(CFLAGS)

server-wrapper_debug: $(HEADERS) $(SERVER).cpp server
	$(CPP) -ggdb -D DEBUG $(SERVER).cpp -o $(SERVER) $(GSFLAGS) $(CFLAGS)
	
device-scanner_debug: $(HEADERS) $(DEV).cpp
	$(CPP) -ggdb -D DEBUG $(DEV).cpp -o $(DEV) $(GSFLAGS) $(CFLAGS)

clean:
	rm $(DEV) $(SERVER)
