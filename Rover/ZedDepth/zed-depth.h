#pragma once
#include <vector>
#include <opencv2/opencv.hpp>
#include <opencv2/features2d.hpp>
#include <opencv2/highgui.hpp>
#include <sl/Camera.hpp>
#include <thread>

// parameters for zed
sl::RuntimeParameters runtime_params;
sl::InitParameters init_params;

// threads used 
std::thread t1;
std::thread t2;
std::thread t3;

sl::Mat img_zed;
sl::Mat depth_img_zed;
cv::Mat depth_img_cv;

std::vector<std::pair<cv::Rect, float> > get_obstacle_get();

int init(const char*);
