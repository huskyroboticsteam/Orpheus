OPENCV_VERSION=4.0.1 # Only works for OpenCV version 4+
ARCH_BIN=6.2 # For Jetson TX2
GCC_VERSION=6 # Version 6 required for CUDA 9
GXX_VERSION=6 # Version 6 required for CUDA 9
WORKING_DIR=$(pwd)

# 1. KEEP UBUNTU OR DEBIAN UP TO DATE

sudo apt-get -y update
sudo apt-get -y upgrade
sudo apt-get -y dist-upgrade
sudo apt-get -y autoremove


# 2. INSTALL THE DEPENDENCIES

# Build tools:
sudo apt-get install -y build-essential cmake

# GUI (if you want to use GTK instead of Qt, replace 'qt5-default' with 'libgtkglext1-dev' and remove '-DWITH_QT=ON' option in CMake):
sudo apt-get install -y qt5-default libvtk6-dev

# Media I/O:
sudo apt-get install -y zlib1g-dev libjpeg-dev libwebp-dev libpng-dev libtiff5-dev libjasper-dev libopenexr-dev libgdal-dev

# Video I/O:
sudo apt-get install -y libdc1394-22-dev libavcodec-dev libavformat-dev libswscale-dev libtheora-dev libvorbis-dev libxvidcore-dev libx264-dev yasm libopencore-amrnb-dev libopencore-amrwb-dev libv4l-dev libxine2-dev

# Parallelism and linear algebra libraries:
sudo apt-get install -y libtbb-dev libeigen3-dev

# Python:
sudo apt-get install -y python-dev python-tk python-numpy python3-dev python3-tk python3-numpy

# Java:
sudo apt-get install -y ant default-jdk

# Documentation:
sudo apt-get install -y doxygen


# 3. INSTALL THE LIBRARY

sudo apt-get install -y unzip wget

wget http://github.com/opencv/opencv/archive/${OPENCV_VERSION}.zip
unzip ${OPENCV_VERSION}.zip
rm ${OPENCV_VERSION}.zip

wget http://github.com/opencv/opencv_contrib/archive/${OPENCV_VERSION}.zip
unzip ${OPENCV_VERSION}.zip
rm ${OPENCV_VERSION}.zip

cd opencv-${OPENCV_VERSION}
mkdir build
cd build

cmake -DCMAKE_C_COMPILER=gcc-${GCC_VERSION} -DCMAKE_CXX_COMPILER=g++-${GXX_VERSION} -DCMAKE_BUILD_TYPE=RELEASE -DCMAKE_INSTALL_PREFIX=/usr/local -DWITH_CUDA=ON -DCUDA_ARCH_BIN=${ARCH_BIN} -DCUDA_ARCH_PTX="" -DWITH_CUBLAS=ON -DWITH_GSTREAMER=ON -DWITH_QT=ON -DWITH_OPENGL=ON -DBUILD_opencv_java=OFF -DOPENCV_GENERATE_PKGCONFIG=ON -DOPENCV_EXTRA_MODULES_PATH=${WORKING_DIR}/opencv_contrib-${OPENCV_VERSION}/modules -DENABLE_FAST_MATH=ON -DCUDA_FAST_MATH=ON -DBUILD_EXAMPLES=OFF -DBUILD_DOCS=OFF -DBUILD_PERF_TESTS=OFF -DBUILD_TESTS=OFF -DWITH_NVCUVID=ON ..

make -j6
sudo make install
sudo ldconfig
