#set Cmake minimum version
cmake_minimum_required (VERSION 2.8)
#defines tennisball project
project (tennisball)

#ensure that the bin directory is created (this is where compiled executables go)
file(MAKE_DIRECTORY ./bin)

#make sure that compiled executables are placed in the bin directory
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ./bin)

#locate the OpenCV library
find_package( OpenCV REQUIRED )
#make sure the OpenCV library directory is included when compiling
include_directories( ${OpenCV_INCLUDE_DIRS} )

#Add the still executable
add_executable( still src/still_image.cpp)

#Add the live executable
add_executable( live src/live_video.cpp)

#add sources for still executable
target_sources( still
  PUBLIC
  src/still_image.cpp
  src/detector.cpp
  src/detector.hpp
  )

#add sources for live executable
target_sources( live
  PUBLIC
  src/live_video.cpp
  src/detector.cpp
  src/detector.hpp
  )

#link the OpenCV library
target_link_libraries( live ${OpenCV_LIBS} )
target_link_libraries( still ${OpenCV_LIBS} )
