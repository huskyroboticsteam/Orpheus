#include <iostream>
#include <string>
#include <cstdint>
#include <vector>
#include <algorithm>
#include <opencv2/opencv.hpp>
#include <opencv2/features2d.hpp>
#include <sl/Camera.hpp>

cv::Mat slMat2cvMat(const sl::Mat &input)
{
    // Mapping between MAT_TYPE and CV_TYPE
    int cv_type = -1;
    switch (input.getDataType())
    {
    case sl::MAT_TYPE_32F_C1: cv_type = CV_32FC1; break;
    case sl::MAT_TYPE_32F_C2: cv_type = CV_32FC2; break;
    case sl::MAT_TYPE_32F_C3: cv_type = CV_32FC3; break;
    case sl::MAT_TYPE_32F_C4: cv_type = CV_32FC4; break;
    case sl::MAT_TYPE_8U_C1: cv_type = CV_8UC1; break;
    case sl::MAT_TYPE_8U_C2: cv_type = CV_8UC2; break;
    case sl::MAT_TYPE_8U_C3: cv_type = CV_8UC3; break;
    case sl::MAT_TYPE_8U_C4: cv_type = CV_8UC4; break;
    default: break;
    }

    // Since cv::Mat data requires a uchar* pointer, we get the uchar1 pointer from sl::Mat (getPtr<T>())
    // cv::Mat and sl::Mat will share a single memory structure
    return cv::Mat(input.getHeight(), input.getWidth(), cv_type, input.getPtr<sl::uchar1>(sl::MEM_CPU));
}

int main(void)
{
    sl::InitParameters init_params;
    init_params.camera_resolution = sl::RESOLUTION_HD1080;
    init_params.depth_mode = sl::DEPTH_MODE_QUALITY;
    init_params.coordinate_units = sl::UNIT_METER;

    sl::Camera zed;
    if(zed.open(init_params) != sl::SUCCESS)
    {
        std::cout << "Failed to open camera.\n";
        return(1);
    }

    sl::RuntimeParameters runtime_params;
    runtime_params.sensing_mode = sl::SENSING_MODE_STANDARD;
    runtime_params.enable_point_cloud = false;
    sl::Resolution image_size = zed.getResolution();
    int32_t new_width = image_size.width / 2;
    int32_t new_height = image_size.height / 2;

    sl::Mat img_zed(new_width, new_height, sl::MAT_TYPE_8U_C4);
    cv::Mat image_cv = slMat2cvMat(img_zed);

    sl::Mat depth_img_zed(new_width, new_height, sl::MAT_TYPE_8U_C4);
    cv::Mat depth_img_cv = slMat2cvMat(depth_img_zed);

    sl::Mat sl_depth_f32;
    cv::Mat cv_depth_f32;

    constexpr int num_pics = 10;    
    
    if(zed.grab(runtime_params) == sl::SUCCESS)
    {
        zed.retrieveImage(img_zed, sl::VIEW_LEFT, sl::MEM_CPU, new_width, new_height);
        zed.retrieveImage(depth_img_zed, sl::VIEW_DEPTH, sl::MEM_CPU, new_width, new_height);
        zed.retrieveMeasure(sl_depth_f32, sl::MEASURE_DEPTH);
        cv::Mat tmp = slMat2cvMat(sl_depth_f32);
        cv_depth_f32 = tmp;
        std::cout << "Frame 1" << std::endl;
    }
    else
    {
        std::cout << "Failed to grab frame" << std::endl;
    }
    for(int i = 0; i < num_pics - 1; i++)
    {
        if(zed.grab(runtime_params) == sl::SUCCESS)
        {
            zed.retrieveImage(img_zed, sl::VIEW_LEFT, sl::MEM_CPU, new_width, new_height);
            zed.retrieveImage(depth_img_zed, sl::VIEW_DEPTH, sl::MEM_CPU, new_width, new_height);
            zed.retrieveMeasure(sl_depth_f32, sl::MEASURE_DEPTH);
            cv::Mat this_cv_depth_f32 = slMat2cvMat(sl_depth_f32);
            cv_depth_f32 += this_cv_depth_f32;
            std::cout << "Frame " << (i + 1) << std::endl;
        }
        else
        {
            std::cout << "Failed to grab frame.\n";
        }
        cv::waitKey(100);
    }
    cv_depth_f32 /= num_pics;
    imwrite("ground.png", cv_depth_f32);
    zed.close();
    return(0);
}
