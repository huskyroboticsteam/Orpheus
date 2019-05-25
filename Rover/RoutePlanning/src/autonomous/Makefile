CC := g++
SDIR := .
ODIR := obj
SOURCES := utils.cpp mapper.cpp timer.cpp memorizer.cpp quadMapper.cpp pather.cpp
_OBJECTS := $(SOURCES:.cpp=.o)
OBJECTS := $(patsubst %,$(ODIR)/%,$(_OBJECTS))
DEBUG := 1

ifeq ($(DEBUG), 1)
CFLAGS := -std=c++11 -Wall -g
else
CFLAGS := -std=c++11 -O3
endif

all: $(OBJECTS)

$(ODIR)/%.o: $(SDIR)/%.cpp $(SDIR)/%.hpp
	$(CC) -c -o $@ $< $(CFLAGS) 
