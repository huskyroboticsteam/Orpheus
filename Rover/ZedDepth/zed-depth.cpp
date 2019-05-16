#include <iostream>
#include <string>
#include <cstdint>
#include <vector>
#include <algorithm>
#include <chrono>
#include <opencv2/opencv.hpp>
#include <opencv2/features2d.hpp>
#include <opencv2/highgui.hpp>
#include <sl/Camera.hpp>
#include <thread>
#include "server.h"
#include <glib.h>
#include "zed-depth.h"

cv::Mat slMat2cvMat(const sl::Mat &input);
sl::Camera zed;
cv::VideoWriter writer;
cv::VideoWriter writer_debug;
int32_t new_width;
int32_t new_height;

void getSomeImages()
{
  sl::Mat img_zed(new_width, new_height, sl::MAT_TYPE_8U_C4);

  for(;;)
  {
    zed.retrieveImage(img_zed, sl::VIEW_LEFT, sl::MEM_CPU, new_width, new_height);
    cv::Mat three_channel_bgr;
    cv::Mat img_cv = slMat2cvMat(img_zed);
    cv::cvtColor(img_cv, three_channel_bgr, cv::COLOR_BGRA2BGR);
    writer.write(three_channel_bgr);
    std::this_thread::sleep_for(std::chrono::milliseconds(20));
  }
}

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

    // Since cv::Mat data requires a uchar* pointer, 
    // we get the uchar1 pointer from sl::Mat (getPtr<T>())
    // cv::Mat and sl::Mat will share a single memory structure
  return cv::Mat(input.getHeight(), 
                 input.getWidth(), 
                 cv_type, 
                 input.getPtr<sl::uchar1>(sl::MEM_CPU)
                );
}

std::vector<std::pair<cv::Rect, float> > get_obstacle_data()
{
  // 
  std::vector<std::pair<cv::Rect, float> > result;

  // get resolution of image from zed api
  sl::Resolution image_size = zed.getResolution();
  new_width = image_size.width;
  new_height = image_size.height;

  // construct img mats
  sl::Mat img_zed(new_width, new_height, sl::MAT_TYPE_8U_C4);

  // construct depth mats
  sl::Mat depth_img_zed(new_width, new_height, sl::MAT_TYPE_8U_C4);
  cv::Mat depth_img_cv = slMat2cvMat(depth_img_zed);

  // retrieve images from zed api
  sl::Mat sl_depth_f32;
  zed.retrieveImage(img_zed, sl::VIEW_LEFT, sl::MEM_CPU, new_width, new_height);
  zed.retrieveImage(depth_img_zed, sl::VIEW_DEPTH, sl::MEM_CPU, new_width, new_height);
  zed.retrieveMeasure(sl_depth_f32, sl::MEASURE_DEPTH);

  std::vector<cv::Vec4i> hierarchy;
  cv::Mat edges, blur_kern;
  std::vector<std::vector<cv::Point> > contours;

  cv::Mat img_cv = slMat2cvMat(img_zed);
#define TIME std::chrono::duration<float, std::milli>(end - start).count()
#define NOW std::chrono::high_resolution_clock::now();
  auto start = NOW;

  // blur image
  cv::cvtColor(img_cv, img_cv, CV_8U, 3);
  cv::Mat img_cv_blur(img_cv.size(), CV_8UC3);
  cv::bilateralFilter(img_cv, img_cv_blur, 9, 150.0, 150.0,  cv::BORDER_DEFAULT);

  // setup for contours for regular image
  cv::resize(img_cv_blur, img_cv_blur, cv::Size(new_width, new_height));
  cv::Canny(img_cv_blur, edges, 100, 200);
            
  auto end = NOW;
  auto ms = TIME;
  //std::cout << "Preprocessing Time: " << ms << " ms\n";
            

  start = NOW;
  // make contours for watershed with 8-bit single-channel image
  cv::findContours(edges, 
                   contours, 
                   hierarchy, 
                   cv::RETR_EXTERNAL, 
                   cv::CHAIN_APPROX_SIMPLE);

  // 
  if(hierarchy.empty()) return result;    

  // make contours to cv mat
  cv::Mat contour_img(edges.size(), CV_32S);
  cv::Mat contour_img_visible(edges.size(), CV_32S);

  // reset contours
  contour_img = 0;
  contour_img_visible = 0;
  int idx = 0;
  int comp_count = 0;

  // make contours
  for(; idx >= 0; idx = hierarchy[idx][0], comp_count++)
  {
    // contours for boundaries of segments
    cv::drawContours(contour_img, 
                     contours, 
                     idx, 
                     cv::Scalar::all(idx), 
                     -1, 
                     8, 
                     hierarchy, 
                     1);

    // contours for objects
    cv::drawContours(contour_img_visible, 
                     contours, 
                     idx, 
                     cv::Scalar::all(INT_MAX), 
                     -1, 
                     8, 
                     hierarchy, 
                     1);
  }

  // convert to cv mat
  cv::Mat cv_depth_f32 = slMat2cvMat(sl_depth_f32);
  cv::Mat depth_f32;
  cv::resize(cv_depth_f32, depth_f32, cv::Size(new_width, new_height));
  cv::Mat three_channel_img;
  cv::cvtColor(img_cv_blur, three_channel_img, cv::COLOR_BGRA2BGR);

  end = NOW;
  ms = TIME;
  //std::cout << "Contours Time: " << ms << " ms\n";

  start = NOW;
  // watershed the image
  cv::watershed(three_channel_img, contour_img);
  cv::Mat wshed(contour_img.size(), CV_8UC3);

  end = NOW;
  ms = TIME;
  //std::cout << "Watershed Time: " << ms << " ms\n";

  start = NOW;
  // for each segment in watershed image make color
  cv::Mat temp, temp_f32;
  std::vector<cv::Vec3b> colors(comp_count);
  std::vector<bool> obstacle(comp_count);
  std::vector<std::pair<cv::Rect, float> > rects(comp_count);
  for(int i = 0; i < comp_count; i++)
  {
    // get color for i segment 
    colors[i] = cv::Vec3b(0, rand() % 255, 0);

    // filter out everthing except i segment
    cv::inRange(contour_img, i, i, temp);
                
    // convert this mat to f32
    temp.convertTo(temp_f32, CV_32F, 1.0f / 255.0f);
                
    // find location that is closest and find distance
    double dist;
    cv::minMaxLoc(depth_f32.mul(temp_f32), nullptr, &dist);
    float z = static_cast<float>(dist);

    // make pair of bounding box and depth in meters
    rects[i] = std::make_pair(cv::boundingRect(temp), z);
                
    // determine if obstacle 
    int nonzero = cv::countNonZero(temp);
    obstacle[i] = 5000 <= nonzero 
                && nonzero <= 200000
                && rects[i].first.y + rects[i].first.height < contour_img.rows - 1
                && z != NAN
                && z > 0
                && z <= 6.0;
  }
  
  // add objects that are obstacles into result         
  for(int i = 0; i < rects.size(); i++)
  {
    if(obstacle[i]) result.push_back(rects[i]);
  }
  
  // print info into rects of objects      
  for(int i = 0; i < result.size(); i++)
  {
    constexpr int thickness = 6;
    //if(!obstacle[i]) continue;
    auto rect = result[i].first;
    cv::rectangle(wshed, rect, cv::Scalar(0, 0, 255), thickness);
    std::ostringstream ss;
    ss << result[i].second << " meters";
    cv::putText(wshed,
                ss.str(),
                cv::Point(rect.x + thickness, rect.y + rect.height - thickness),
                cv::FONT_HERSHEY_SIMPLEX,
                0.4,
                cv::Scalar(0, 255, 0));

  }
  // end = NOW;
  // ms = TIME;
  //std::cout << "Coloring Time: " << ms << " ms\n";
  //std::cout << std::endl;
           
  //cv::imshow("watershed", wshed);
  return result;
}

int init(const char* camera_path)
{
  // setup params for zed
  //sl::InitParameters init_params;
  init_params.camera_resolution = sl::RESOLUTION_VGA;
  init_params.depth_mode = sl::DEPTH_MODE_QUALITY;
  init_params.coordinate_units = sl::UNIT_METER;

  if(zed.open(init_params) != sl::SUCCESS)
  {
    std::cout << "Failed to open camera.\n";
    return 1;
  }

  runtime_params.sensing_mode = sl::SENSING_MODE_STANDARD;
  runtime_params.enable_point_cloud = false;
  sl::Resolution image_size = zed.getResolution();

  // scale image ?
  //new_width = image_size.width / 2;
  //new_height = image_size.height / 2;

  new_width = image_size.width;
  new_height = image_size.height;

  img_zed = sl::Mat(new_width, new_height, sl::MAT_TYPE_8U_C4);

  // depth images
  sl::Mat depth_img_zed(new_width, new_height, sl::MAT_TYPE_8U_C4);
  cv::Mat depth_img_cv = slMat2cvMat(depth_img_zed);

  sl::Mat sl_depth_f32;

  // gstreamer 
  g_server_data data;
  data.argc = 5;
  data.argv[0] = camera_path;
  data.argv[1] = "intervideosrc";
  data.argv[2] = "zed_depth";
  data.argv[3] = "5556";
  data.argv[4] = "intervideosrc channel=rgb ! rtpvrawpay name=pay0 pt=96";
    
  writer.open("appsrc ! video/x-raw,format=BGR ! videoconvert ! video/x-raw,format=I420 ! intervideosink channel=rgb", 
              0, 
              10, 
              cv::Size(img_zed.getWidth(), 
              img_zed.getHeight()), 
              true);

  // thread to ...
  t1 = std::thread(start_server, data.argc, (char **) data.argv);
 
  // thread to ...
  t2 = std::thread(getSomeImages);

#ifdef DEBUG
  // gstreamer
  g_server_data data2;
  data2.argc = 5;
  data2.argv[0] = camera_path;
  data2.argv[1] = "intervideosrc";
  data2.argv[2] = "zed_depth_debug";
  data2.argv[3] = "8888";
  data2.argv[4] = "intervideosrc channel=wshed ! rtpvrawpay name=pay0 pt=96";
 
  writer_debug.open("appsrc ! video/x-raw,format=BGR ! videoconvert ! video/x-raw,format=I420 ! intervideosink channel=wshed", 
                    0, 
                    10, 
                    cv::Size(img_zed.getWidth(), 
                    img_zed.getHeight()), 
                    true);
  
  // thread to ...
  t3 = std::thread(start_server, data2.argc, (char **) data2.argv);
#endif
  return 0;
}

int main(int argc, char *argv[])
{
  if (init(argv[1]) != 0) printf("cock\n");
  for(char key = ' '; key != 'q'; key = cv::waitKey(10))
  {
    if(zed.grab(runtime_params) == sl::SUCCESS)
    {
      std::vector<std::pair<cv::Rect, float> > obstacles = get_obstacle_data();
      //end = NOW;
      //ms = TIME;
      //std::cout << "Obstacles Time: " << ms << " ms\n";

      //start = NOW;
      // put color in segment
      /*cv::Mat wshed_mask(contour_img.size(), CV_8UC3);
      for(int i = 0; i < contour_img.rows; i++)
        for(int j = 0; j < contour_img.cols; j++)
        {
          int index = contour_img.at<int>(i, j);

          if(index == -1)
          wshed.at<cv::Vec3b>(i, j) = cv::Vec3b(255, 255, 255);

          else if(index <= 0 || index > comp_count || !obstacle[index])
          wshed.at<cv::Vec3b>(i, j) = cv::Vec3b(0, 0, 0);   
                 
          else
          wshed.at<cv::Vec3b>(i, j) = colors[index];
        }*/
      for(auto o : obstacles) 
      {
      std::cout << "x: " << o.first.x 
                << " y: " << o.first.y 
                << " width:  " << o.first.width 
                << " height:  " << o.first.height  
                << " distance: " << o.second  
                <<  std::endl;
      }

      printf("\n");
 
#ifdef DEBUG
      writer_debug.write(wshed);
#endif
#if 0
      cv::Mat three_channel;
      cv::cvtColor(depth_img_cv, three_channel, cv::COLOR_BGRA2BGR);
      cv::applyColorMap(three_channel, bgr, cv::COLORMAP_JET);
      cv::imshow("Original", image_cv);
      cv::imshow("Depth", depth_img_cv);
      cv::imshow("Depth", bgr);
      cv::imshow("Depth2", cv_depth_u8);
      cv::imshow("Point Cloud", point_cloud_cv);
      cv::imshow("Depth Measure", cv_depth_f32);
#endif
    }
    else std::cout << "Failed to grab frame.\n";
  }
    
  zed.close();
  return(0);
}
