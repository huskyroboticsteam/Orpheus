#ifndef DETECTOR_H
#define DETECTOR_H

#include <string>
#include <opencv2/core.hpp>
#include <opencv2/dnn.hpp>

using cv::Mat;
namespace tb {
    class Detection {
    private:
        int left;
        int right;
        int top;
        int bottom;
        float confidence;
    public:
        Detection(int left, int right, int top, int bottom, float confidence); 

    public:
        float getConfidence();
        float getConfidencePct();
        cv::Rect2i getBBoxRect();
        cv::Point2f getBBoxCenter();
        int getBBoxLeft();
        int getBBoxRight();
        int getBBoxTop();
        int getBBoxBottom();
        int getBBoxWidth();
        int getBBoxHeight();
    };

    class Detector {
      private:
        cv::dnn::Net network;

      public:
        std::vector<Detection> performDetection(cv::Mat image);
        std::vector<Detection> performDetection(cv::Mat image, float confidenceThreshold);
        Detector(std::string binaryGraphPath, std::string graphPbtxtPath);
    };
} // namespace tb

#endif
