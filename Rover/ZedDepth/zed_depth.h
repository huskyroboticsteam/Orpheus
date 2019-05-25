#pragma once
#include <vector>
#include <utility>
#include <opencv2/opencv.hpp>

struct ObstacleDetection
{
    cv::Mat image;
    std::vector<std::pair<cv::Rect, float> > obstacles;
};

ObstacleDetection get_obstacle_data();

//int init(const char*);
int zdInit();
int gsInit(const char*); 
