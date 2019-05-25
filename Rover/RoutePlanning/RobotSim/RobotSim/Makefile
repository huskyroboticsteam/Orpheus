CC = g++
SDIR = .
ODIR = obj
SOURCES = Controller.cpp Server.cpp RobotEKF.cpp
_OBJECTS = $(SOURCES:.cpp=.o)
OBJECTS = $(patsubst %,$(ODIR)/%,$(_OBJECTS))
GSFLAGS= `pkg-config --cflags --libs gstreamer-1.0` -lgstrtspserver-1.0
CFLAGS = -std=c++14 -g
OUT = build/routeplanning
OTHER_OBJECTS = Tennisball/CMakeFiles/still.dir/src/detector.cpp.o  ../../../ZedDepth/zed_depth.o ../../../GStreamer/server.o

INCLUDE = -I/usr/include -Ikalman `pkg-config --libs opencv4` -I/usr/local/zed/include -I../../../GStreamer -I/usr/local/cuda/include -I/usr/local/include/opencv4
LINK = -L/usr/local/lib  -lopencv_core -lopencv_dnn -lpthread `pkg-config --cflags --libs gstreamer-1.0` `pkg-config --cflags --libs opencv4` -std=c++14 /usr/local/zed/lib/*.so -march=armv8-a+simd $(GSFLAGS)

AUTO_OBJ := $(shell find autonomous/obj/ -name '*.o')

all: make_gstreamer make_zed_depth make_auto make_tennis $(OUT) 

make_gstreamer:
	make server-wrapper -C ../../../GStreamer

make_auto:
	make -C autonomous

make_tennis:
	make -C Tennisball

make_zed_depth:
	make zed_depth-only -C ../../../ZedDepth

$(ODIR)/%.o: $(SDIR)/%.cpp
	$(CC) -c -o $@ $< $(CFLAGS) $(INCLUDE)

$(OUT): $(OBJECTS)
	$(CC) $(CFLAGS) $(GSFLAGS) $(OBJECTS) $(AUTO_OBJ) $(OTHER_OBJECTS) $(LINK) $(INCLUDE) -o $(OUT)

.PHONY: clean, make_auto, make_tennis, make_zed_depth

clean:
	rm $(OUT)

