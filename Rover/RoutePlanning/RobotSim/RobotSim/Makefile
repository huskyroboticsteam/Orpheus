CC := g++
SDIR := .
ODIR := obj
COMMON_SOURCES := main.cpp agent.cpp grid.cpp obstacle.cpp simController.cpp Simulator.cpp ui.cpp interface.cpp RobotEKF.cpp
NET_SOURCES := WorldCommunicator.cpp
_COMMON_OBJECTS := $(COMMON_SOURCES:.cpp=.o)
_NET_OBJECTS := $(NET_SOURCES:.cpp=.o)
COMMON_OBJECTS := $(patsubst %,$(ODIR)/%,$(_COMMON_OBJECTS))
NET_OBJECTS := $(patsubst %,$(ODIR)/%,$(_NET_OBJECTS))
DEBUG ?= 1
LOCAL ?= 1

AUTO_DIR := autonomous/
AUTO_ODIR := $(AUTO_DIR)obj
AUTO_SOURCES := $(wildcard $(AUTO_DIR)*.cpp)
_AUTO_OBJECTS := $(notdir $(AUTO_SOURCES:.cpp=.o))
AUTO_OBJECTS := $(patsubst %,$(AUTO_ODIR)/%,$(_AUTO_OBJECTS))

SFML := C:/SFML-2.5.1/
INC := -I$(SFML)include
LINK := -L$(SFML)lib -lsfml-graphics -lsfml-audio -lsfml-window -lsfml-system -lpthread

ifeq ($(DEBUG), 1)
CFLAGS := -std=c++11 -Wall -g
else
CFLAGS := -std=c++11 -DNDEBUG -O3
endif

ifeq ($(LOCAL), 1)
SOURCES := $(COMMON_SOURCES)
OBJECTS := $(COMMON_OBJECTS)
CFLAGS := $(CFLAGS) -DLOCAL
else
SOURCES := $(COMMON_SOURCES) $(NET_SOURCES)
OBJECTS := $(COMMON_OBJECTS) $(NET_OBJECTS)
endif
OUT := build/simulator.exe

all: make-auto $(OUT)

make-auto:
	$(MAKE) -C $(AUTO_DIR) DEBUG=$(DEBUG)
	

$(ODIR)/%.o: $(SDIR)/%.cpp $(SDIR)/%.hpp
	$(CC) -c $(INC) -o $@ $< $(CFLAGS) 

$(OUT): $(OBJECTS) $(AUTO_OBJECTS)
	$(CC) $(CFLAGS) $(AUTO_OBJECTS) $(OBJECTS) $(LINK) -o $(OUT)

print-%  : ; @echo $* = $($*)

.PHONY: clean make-auto
clean:
	rm -f $(OUT)
	rm -f obj/*
	rm -f $(AUTO_DIR)obj/*
