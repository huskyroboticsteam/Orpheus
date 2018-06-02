#include <stdio.h>
#include <gst/gst.h>
#include "gserver.h"
#include <opencv2/opencv.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <iostream>
#include <string>

using namespace cv;
using namespace std;

//Found these through experimentation.
const int COLS = 572;
const int ROWS = 592;

std::pair<int, int> coordinates[COLS][ROWS];

/*
   Splits the provided image into two. Splitting occurs at the middle of width.
   Returns a pair of Mat objects. First is the left half. Second is right.
 */
std::pair<Mat,Mat> split(Mat dualFisheye)
{	
  //----------------------------------------------------------------------------------------
  //DO NOT MESS WITH THESE VALUES UNLESS TOTALLY NECESSARY. THEY HAVE BEEN SELECTED FOR OUR 
  //PARTICULAR RICOH THETEA S THROUGH EXPERIMENTATION.
  //----------------------------------------------------------------------------------------
  cv::Rect roi;
  roi.x = 15;
  roi.y = 36;
  roi.width = dualFisheye.cols / 2 - 48; // ROWS
  roi.height = dualFisheye.rows - 148;   // COLS
  //Crop the image using these parameters.
  Mat left = dualFisheye(roi);
  //Crop the other half.
  roi.x = dualFisheye.cols / 2 + 31;
  roi.y = 44;
  roi.width = dualFisheye.cols / 2 - 48;
  roi.height = dualFisheye.rows - 148 ;
  Mat right = dualFisheye(roi);
  return std::make_pair(left, right);
}

/*
   This will take separated images from the feed of the Ricoh Theta S. It will
   straighten the images to get the actual top facing up. 
   Accepts a pair of images. The first is the left feed and second is right feed. 
 */
std::pair<Mat, Mat> correctOrientation(std::pair<Mat, Mat> feed)
{
  transpose(feed.first, feed.first);
  flip(feed.first, feed.first, +1);
  transpose(feed.second, feed.second);
  flip(feed.second, feed.second, 0);
  return feed;
}

/*
   This will do the transformation from the circular to rectangular field.
   Will be using the maps stored in coordinates pair of array.
 */
std::pair<Mat, Mat> fisheyeToRect(std::pair<Mat, Mat> feed)
{
  Mat leftSq(feed.first.rows, feed.first.cols, CV_8UC3);
  Mat rightSq(feed.second.rows, feed.second.cols, CV_8UC3);
  for (int y = 0; y < ROWS; y++) {
    for (int x = 0; x < COLS; x++) {
      //The vector goes rows X cols
      leftSq.at<Vec3b>(y, x) = feed.first.at<Vec3b>(coordinates[x][y].second, coordinates[x][y].first);
      rightSq.at<Vec3b>(y, x) = feed.second.at<Vec3b>(coordinates[x][y].second, coordinates[x][y].first);
    }
  }
  return std::make_pair(leftSq, rightSq);
}

/*
   Will take a feed and will put them together in one Mat object.
   Will return this new image.
 */
Mat stitch(std::pair<Mat, Mat> feed)
{
  Mat pano(feed.first.rows, feed.first.cols * 2, CV_8UC3);
  feed.first.copyTo(pano(Rect(0, 0, feed.first.cols, feed.first.rows)));
  feed.second.copyTo(pano(Rect(feed.first.cols, 0, feed.second.cols, feed.second.rows)));
  return pano;
}

/*
   Reads the remap file and writes the pixel maps to coordinates.
   Files were generated using a java program remapping.java. It is in the same
   directory as this program.
 */
void saveToArray() 
{
  std::ifstream X("remap_x.txt");
  std::ifstream Y("remap_y.txt");
  std::string lineX;
  std::string lineY;
  int row = 0;
  while (std::getline(X, lineX) && std::getline(Y, lineY))
  {
    std::stringstream streamX(lineX);
    std::stringstream streamY(lineY);
    int col = 0;
    while (col < COLS)
    {
      int x;
      streamX >> x;
      int y;
      streamY >> y;
      if (x > COLS)
      {
        x = COLS;
      }
      if (y > ROWS)
      {
        y = ROWS;
      }
      coordinates[col][row] = std::make_pair(x, y);
      col++;
    }
    row++;
  }
}

int r_start(g_element ele, gchar *ip) {
  const gchar *path = gst_structure_get_string(ele->str, "device.path");

  // string for getting image data from ricoh
  std::string inward_stream;
  inward_stream += "v4l2src device=" + std::string(path) + " ! video/x-raw, format=BGR, width=1280, height=720 ! videorate ! video/x-raw, framerate=(fraction)10/1 ! appsink";

  // string for sending 
  std::string outward_stream;
  outward_stream += "appsrc ! autovideoconvert ! omxh264enc ! video/x-h264, stream-format=(string)byte-stream ! h264parse ! rtph264pay ! udpsink host=" + std::string(ip) + "port=5555";
  // creating the capture 
  VideoCapture cap(inward_stream, CAP_GSTREAMER);
  Mat dualFisheye;
  cap.read(dualFisheye);

  if (dualFisheye.empty()) {
    cout << "There was an error getting dual fisheye from the camera" << endl;
    exit(0);
  }
  g_print("made the cap\n");
  saveToArray();

  //Found these values through testing.
  Size frame_size(1144, 592);
  int frames_per_second = 10;
  //Create and initialize the VideoWriter object 
  cv::VideoWriter writer;
  writer.open(outward_stream, CAP_GSTREAMER, 0, 10, Size(1144, 592), true);

  // namedWindow("image Final", WINDOW_AUTOSIZE);

  while (true)
  {
    cap.read(dualFisheye);
    //separate the feed and crop to bounding square
    std::pair<Mat, Mat> separated = split(dualFisheye);
    //correct orientation of the separated feed.
    separated = correctOrientation(separated);
    //Convert the separated feed to rectangular.
    separated = fisheyeToRect(separated);
    //Stitch the two feeds together
    Mat pano = stitch(separated);
    // imshow("image Final", pano);
    writer.write(pano);
    if (waitKey(30) == 27) break;
  }
  writer.release();
  return 0;
}
