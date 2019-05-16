CC = g++
SDIR = .
ODIR = obj
SOURCES = Controller.cpp Server.cpp RobotEKF.cpp
_OBJECTS = $(SOURCES:.cpp=.o)
OBJECTS = $(patsubst %,$(ODIR)/%,$(_OBJECTS))
CFLAGS = -std=c++11 -g
OUT = build/routeplanning
OTHER_OBJECTS = Tennisball/CMakeFiles/still.dir/src/detector.cpp.o  ../../../ZedDepth/zed-depth.o

INCLUDE = -I/usr/include
LINK = -L/usr/local/lib -lopencv_core -lopencv_core -lopencv_dnn -lpthread

AUTO_OBJ := $(shell find autonomous/obj/ -name '*.o')

all: make_auto $(OUT) make_tennis

make_auto:
	make -C autonomous

make_tennis:
	make -C Tennis

make_zed_depth:
	make -C ../../../ZedDepth

$(ODIR)/%.o: $(SDIR)/%.cpp
	$(CC) -c -o $@ $< $(CFLAGS) 

$(OUT): $(OBJECTS)
	$(CC) $(CFLAGS) $(OBJECTS) $(AUTO_OBJ) $(OTHER_OBJECTS) $(LINK) $(INCLUDE)

.PHONY: clean, make_auto, make_tennis, make_zed_depth
clean:
	rm $(OUT)

