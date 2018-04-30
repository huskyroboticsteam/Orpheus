#include "videowidget.h"
#include <QPainter>

#define stream "udpsrc caps=\"application/x-rtp, media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\" port=5555 ! rtpjitterbuffer ! rtph264depay ! h264parse ! openh264dec ! appsink"

using namespace cv;
using namespace std;

VideoWidget::VideoWidget(QWidget *parent) : QWidget(parent), timer(this), capture(stream, CAP_GSTREAMER)
{
    connect(this->timer, SIGNAL(timeout()), this, SLOT(queryFrame()));
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
    painter.drawImage(point, *(this->image));
}

QImage *
VideoWidget::buildImage(Mat in_frame)
{

    CvSize size(in_frame.size().width, in_frame.size().width);
    IplImage *input_frame = cvCreateImage(size, IPL_DEPTH_8U, in_frame.channels());
    cvSetData(input_frame, in_frame.data, (int) (in_frame.total() * in_frame.elemSize()));

    if (!this->frame) {
        CvSize size(input_frame->width, input_frame->height);
        this->frame = cvCreateImage(size, IPL_DEPTH_8U, input_frame->nChannels);
    }

    if (input_frame->origin == IPL_ORIGIN_TL) {
        cvCopy(input_frame, this->frame);
    } else {
        cvFlip(input_frame, this->frame, 0);
    }

    Mat alpha(this->frame->width, this->frame->height, CV_8UC1); // convert to CV_8UC1
    Rect r (0, 0, this->frame->width, this->frame->height);
    Scalar color(255, 255, 255);
    rectangle(alpha, r, color, -1);
    Mat rgba (this->frame->height, this->frame->width, CV_8UC4);
    Scalar map(1, 2, 3, 4);
    cvSet(&rgba, map);

    Mat inp = cvarrToMat(this->frame, true);
    vector<Mat> first;
    first.push_back(inp);
    first.push_back(alpha);
    InputArray a(first);

    vector<Mat> second;
    second.push_back(rgba);
    InputArray g(second);

    vector<int> v;
    v.push_back(0);
    v.push_back(0);
    v.push_back(1);
    v.push_back(1);
    v.push_back(2);
    v.push_back(2);
    v.push_back(3);
    v.push_back(4);

    mixChannels(a, g, v);

    return new QImage(rgba.data, this->frame->width, this->frame->height, QImage::Format_RGB32);;
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
