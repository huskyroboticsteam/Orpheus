/**
  @file videowriter_basic.cpp
  @brief A very basic sample for using VideoWriter and VideoCapture
  @author PkLab.net
  @date Aug 24, 2016
*/

#include <opencv2/core.hpp>
#include <opencv2/videoio.hpp>
#include <opencv2/highgui.hpp>
#include <iostream>
#include <thread>
#include <stdio.h>
#include "server.h"

using namespace cv;
using namespace std;

int main(int argc, char* argv[])
{
    Mat src;
    // use default camera as video source
    VideoCapture cap("videotestsrc ! video/x-raw,width=1280,height=720,framerate=30/1 ! appsink sync=false");
    // check if we succeeded
    if (!cap.isOpened()) {
        cerr << "ERROR! Unable to open camera\n";
        return -1;
    }
    // get one frame from camera to know frame size and type
    cap >> src;
    // check if we succeeded
    if (src.empty()) {
        cerr << "ERROR! blank frame grabbed\n";
        return -1;
    }
    bool isColor = (src.type() == CV_8UC3);

    //--- INITIALIZE VIDEOWRITER
    VideoWriter writer;
    double fps = 30.0;                          // framerate of the created video stream
    string filename = "appsrc ! autovideoconvert ! video/x-raw,format=I420 ! intervideosink";
    writer.open(filename, cv::CAP_GSTREAMER, 0, fps, src.size(), isColor);
    // check if we succeeded
    if (!writer.isOpened()) {
        cerr << "Could not open the output video file for write\n";
        return -1;
    }

    const char * test[5];
    test[0] = argv[0];
    test[1] = "intervideosrc";
    test[2] = ".";
    test[3] = "8888";
    test[4] = "intervideosrc ! autovideoconvert ! rtpvrawpay name=pay0 pt=96";    

    thread t1(start_server, 5, (char **) test); 

    //--- GRAB AND WRITE LOOP
    cout << "Writing videofile: " << filename << endl
         << "Press any key to terminate" << endl;
    for (;;)
    {
        // check if we succeeded
        if (!cap.read(src)) {
            cerr << "ERROR! blank frame grabbed\n";
            break;
        }
        // encode the frame into the videofile stream
        writer.write(src);
        // show live and wait for a key with timeout long enough to show images
        //imshow("Live", src);
        //if (waitKey(5) >= 0)
        //    break;
    }
    // the videofile will be closed and released automatically in VideoWriter destructor
    return 0;
}
