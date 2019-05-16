#pragma once
#include <vector>
#include <opencv2/opencv.hpp>
#include <opencv2/features2d.hpp>
#include <opencv2/highgui.hpp>
#include <sl/Camera.hpp>
#include <thread>

std::vector<std::pair<cv::Rect, float> > get_obstacle_data();

int zdInit(const char*);

