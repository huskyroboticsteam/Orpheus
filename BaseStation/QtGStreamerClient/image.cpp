#include "image.h"

using namespace cv;
using namespace std;

Image::Image(Mat frame) : QImage()
{
    Mat alpha = frame; // convert to CV_8UC1
    Rect r (0, 0, frame.size().width, frame.size().height);
    Scalar color(255, 255, 255);
    rectangle(alpha, r, color, -1);
    Mat rgba (frame.size().height, frame.size().width, CV_8UC4);
    //set(rgba, 1, 2, 3, 4);
    //mixChannels();

    //rgba
    QImage::
}

std::string Image::_matToString(Mat &frame) {
    int total = frame.size().width * frame.size().height * frame.channels();
    std::vector<uchar> data(frame.ptr(), frame.ptr() + total);
    std::string s = new std::string(data.begin(), data.end());
    return s;
}
