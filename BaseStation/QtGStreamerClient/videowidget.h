#ifndef VIDEOWIDGET_H
#define VIDEOWIDGET_H

#include <QWidget>
#include <QTimer>
#include <opencv2/opencv.hpp>

class VideoWidget : public QWidget
{
    Q_OBJECT
public:
    explicit VideoWidget(QWidget *parent = nullptr);
    ~VideoWidget();

private:
    void paintEvent(QPaintEvent *event);
    QImage buildImage(cv::Mat frame);
    void queryFrame();

private:
    QImage image;
    QTimer timer;
    cv::VideoCapture capture;

signals:

public slots:
};

#endif // VIDEOWIDGET_H
