#include "detector.hpp"

const int TENNIS_BALL_OBJECT_CLASS = 1;
const double DEFAULT_DETECTION_THRESHOLD = 0.2;
const std::string classnames[] = { "", "TennisBall" };

using namespace tb;

std::vector<Detection> Detector::performDetection(cv::Mat image){
    return performDetection(image, DEFAULT_DETECTION_THRESHOLD);
}
std::vector<Detection> Detector::performDetection(cv::Mat image, float confidenceThreshold) {
    cv::Mat blob = cv::dnn::blobFromImage(image, 1.0, cv::Size(300, 300), cv::Scalar(), true, false);
    network.setInput(blob);

    std::vector<Detection> detections;

    // time to run the detection
    cv::Mat output = network.forward();
    cv::Mat detectionMat(output.size[2], output.size[3], CV_32F, output.ptr<float>());

    for(int i = 0; i < detectionMat.rows; i++) {
        float confidence = detectionMat.at<float>(i, 2);
        if(confidence > confidenceThreshold) {
            int objectClass = static_cast<int>(detectionMat.at<float>(i, 1));
            if(objectClass == TENNIS_BALL_OBJECT_CLASS) {
                int left = static_cast<int>(detectionMat.at<float>(i, 3) * image.cols);
                int top = static_cast<int>(detectionMat.at<float>(i, 4) * image.rows);
                int right = static_cast<int>(detectionMat.at<float>(i, 5) * image.cols);
                int bottom = static_cast<int>(detectionMat.at<float>(i, 6) * image.rows);

                Detection detection (left, right, top, bottom, confidence);
                detections.push_back(detection);
            }
        }
    }
    return detections;
}

Detector::Detector(std::string binaryGraphPath, std::string graphPbtxtPath) {
    Detector::network = cv::dnn::readNetFromTensorflow(binaryGraphPath, graphPbtxtPath);
}

/*
 *  Detection class
 */
Detection::Detection(int left, int right, int top, int bottom, float confidence) {
    Detection::left = left;
    Detection::right = right;
    Detection::top = top;
    Detection::bottom = bottom;
    Detection::confidence = confidence;
}

float Detection::getConfidence() {
    return confidence;
}

float Detection::getConfidencePct() {
    return confidence * 100;
}

int Detection::getBBoxLeft() {
    return left;
}

int Detection::getBBoxTop() {
    return top;
}

int Detection::getBBoxRight() {
    return right;
}

int Detection::getBBoxBottom() {
    return bottom;
}

int Detection::getBBoxWidth() {
    return right - left;
}

int Detection::getBBoxHeight() {
    return bottom - top;
}

cv::Rect2i Detection::getBBoxRect() {
    cv::Rect2i rect(left, top, getBBoxWidth(), getBBoxHeight());
    return rect;
}
cv::Point2f Detection::getBBoxCenter() {
    float centerX = getBBoxWidth() / 2;
    float centerY = getBBoxHeight() / 2;
    cv::Point2f center(centerX, centerY);
    return center;
}
