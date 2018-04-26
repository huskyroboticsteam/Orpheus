#ifndef IMAGE_H
#define IMAGE_H

#include <QImage>
#include <opencv2/opencv.hpp>

class Image : public QImage
{
public:
    explicit Image(cv::Mat frame);

private:
    std::string _matToString(cv::Mat &frame);
};

#endif // IMAGE_H
