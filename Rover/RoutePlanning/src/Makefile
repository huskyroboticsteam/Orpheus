CC = g++
SDIR = .
ODIR = obj
SOURCES = Controller.cpp Server.cpp RobotEKF.cpp
_OBJECTS = $(SOURCES:.cpp=.o)
OBJECTS = $(patsubst %,$(ODIR)/%,$(_OBJECTS))
CFLAGS = -std=c++11 -g
OUT = build/routeplanning

AUTO_OBJ := $(shell find autonomous/obj/ -name '*.o')

all: make_auto $(OUT)

make_auto:
	make -C autonomous

$(ODIR)/%.o: $(SDIR)/%.cpp
	$(CC) -c -o $@ $< $(CFLAGS) 


$(OUT): $(OBJECTS)
	$(CC) $(CFLAGS) $(OBJECTS) $(AUTO_OBJ) Tennisball/CMakeFiles/still.dir/src/detector.cpp.o -L/usr/local/lib -I/usr/include  -lopencv_core -lopencv_dnn -o $(OUT) -lpthread

.PHONY: clean, make_auto
clean:
	rm $(OUT)
