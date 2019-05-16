TARGET := libkalman.a
DEBUG_FLAG := no

LIB_INSTALL_PATH := /usr/local/lib
INC_INSTALL_PATH := /usr/local/include

ARCHIVE_NAME := $(TARGET:.a=.tar.gz)
INCLUDE_DIR := -I`pwd`

# Define a variable for each type of files
SOURCES := $(wildcard *.cpp)
ALLSOURCES := $(SOURCES)
ALLSOURCES += $(wildcard *.h*)
ALLFILES := $(ALLSOURCES) makefile
OBJECTS := $(SOURCES:.cpp=.o)

# Define a variable for each command
CXX := g++
AR := ar
RM := rm -f
TAR := tar czf
UNIX2DOS := unix2dos
DOS2UNIX := dos2unix
UPDATE := cvs update
COMMIT := cvs commit
INSTALL := install

# Define conditional flags
WARNING_FLAGS := -Wall -W -Wfloat-equal -Winline -Wno-deprecated
ifeq ($(DEBUG_FLAG),yes)
  OTHER_FLAGS := -g
  INSTALL_FLAGS :=
else
  OTHER_FLAGS := -O3 -DNDEBUG
  INSTALL_FLAGS := -s
endif

CXXFLAGS := $(WARNING_FLAGS) $(OTHER_FLAGS) $(INCLUDE_DIR)

# Define some rules
%.o : %.cpp
	$(CXX) -c $(CXXFLAGS) -o $@ $< 

# Define main target
$(TARGET) : $(OBJECTS)
	$(AR) -r $@ $^

# Define some commands
.PHONY : clean toDos toUnix cvs archive install doc develdoc samples

clean :
	$(RM) $(OBJECTS) $(TARGET) *~ kalman/*~
	$(RM) -r doc/public/* doc/private/*

toDos :
	$(UNIX2DOS) $(ALLFILES)

toUnix :
	$(DOS2UNIX) $(ALLFILES)

cvs :
	$(UPDATE)
	$(COMMIT)

archive :
	$(TAR) $(ARCHIVE_NAME) $(ALLFILES)

install :
	$(INSTALL) $(INSTALL_FLAGS) -m 644 $(TARGET) $(LIB_INSTALL_PATH)/$(TARGET)
	$(INSTALL) -d -m 755 $(INC_INSTALL_PATH)/kalman
	$(INSTALL) -m 644 kalman/*.hpp $(INC_INSTALL_PATH)/kalman/

doc :
	doxygen public_doc

develdoc :
	doxygen private_doc

samples :
	./compile_samples.sh

cleansamples:
	./clean_samples.sh

# Define dependencies
kstatics.o : $(ALLSOURCES)
