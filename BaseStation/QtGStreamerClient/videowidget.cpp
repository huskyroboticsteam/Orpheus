#include "videowidget.h"
#include <QPainter>

#define stream "udpsrc caps=\"application/x-rtp, media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=5555 ! rtpjitterbuffer ! rtph264depay ! h264parse ! openh264dec ! appsink"

using namespace cv;

VideoWidget::VideoWidget(QWidget *parent) : QWidget(parent), timer(this), capture(stream, CAP_GSTREAMER)
{
    this->timer.timeout(this->queryFrame);
    this->timer.start(50);
}

VideoWidget::~VideoWidget()
{

}

void
VideoWidget::paintEvent(QPaintEvent *event)
{
    QPainter painter(this);
    QPoint point(0, 0);
    painter.drawImage(point, this->image);
}

QImage
VideoWidget::buildImage(Mat frame)
{
//    Mat output;

//    if (frame.origin == IPL_ORIGIN_TL) {
//        frame.copyTo(output);
//    } else {
//        flip(frame, output, 0);
//    }

    Mat alpha = frame; // convert to CV_8UC1
    Rect r (0, 0, frame.size().width, frame.size().height);
    Scalar color(255, 255, 255);
    rectangle(alpha, r, color, -1);
    Mat rgba (frame.size().height, frame.size().width, CV_8UC4);

}

void
VideoWidget::queryFrame()
{
    Mat frame;
    this->capture.read(frame);
    this->image = this->buildImage(frame);
    this->update();
}

////set(rgba, 1, 2, 3, 4);
////mixChannels();

////rgba.tostring
//QImage::
//}

//std::string Image::_matToString(Mat &frame) {
//int total = frame.size().width * frame.size().height * frame.channels();
//std::vector<uchar> data(frame.ptr(), frame.ptr() + total);
//std::string s = new std::string(data.begin(), data.end());
//return s;
//}
