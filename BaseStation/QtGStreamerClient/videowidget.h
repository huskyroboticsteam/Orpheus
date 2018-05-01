#ifndef VIDEOWIDGET_H
#define VIDEOWIDGET_H

#include <QWidget>
#include <QTimer>
#include <QImage>
#include <opencv2/opencv.hpp>

class VideoWidget : public QWidget
{
    Q_OBJECT
public:
    explicit VideoWidget(QWidget *parent = nullptr);
    ~VideoWidget();

private:
    void paintEvent(QPaintEvent *event);
    QImage * buildImage(cv::Mat in_frame);

private:
    QImage *image;
    QTimer timer;
    cv::VideoCapture *capture;
    IplImage *frame;

signals:

public slots:
    void queryFrame();

};

#endif // VIDEOWIDGET_H
