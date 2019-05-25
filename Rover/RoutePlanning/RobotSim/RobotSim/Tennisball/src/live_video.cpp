#include "detector.hpp"
#include <iostream>
#include <opencv2/core.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/videoio.hpp>

using namespace cv;
using namespace std;

const string WINDOW_TITLE = "Detection";
const string GRAPH_PATH = "./data/final_models/frozen_inference_graph.pb";
const string GRAPH_PBTXT = "./data/final_models/graph.pbtxt";

int main(int argc, char** argv) {
    int deviceID = 0; // 0 = open default camera
    if(argc > 1) {
        // second argument will be the device id
        string device = argv[1];
        deviceID = stoi(device);
    }

    Mat frame; // define a Mat to be used for the frames coming from the camera

    VideoCapture cap; // Initialize VideoCapture, this will be used for the camera

    int apiID = cv::CAP_ANY; // 0 = autodetect default API

    cap.open(deviceID + apiID); // open selected camera using selected API
    if(!cap.isOpened())         // check if we succeeded
    {
        cerr << "ERROR! Unable to open camera\n"; // if not, print an error
        return -1;                                // and exit the program
    }

    tb::Detector detector(GRAPH_PATH, GRAPH_PBTXT);

    //--- GRAB AND WRITE LOOP
    cout << "Start grabbing" << endl << "Press any key to terminate" << endl;
    while(true) {

        cap.read(frame);  // wait for a new frame from camera and store it into 'frame'
        if(frame.empty()) // check if we succeeded
        {
            cerr << "ERROR! blank frame grabbed\n";
            break;
        }

        Mat blur;
        GaussianBlur(frame, blur, Size(5, 5), 0);

        vector<tb::Detection> detections = detector.performDetection(blur);
        for(tb::Detection current : detections) {
            cout << "confidence: " << current.getConfidencePct() << "%" << endl;
            rectangle(blur, current.getBBoxRect(), Scalar(0, 255, 0), 2);
        }

        imshow(WINDOW_TITLE, blur);

        if(waitKey(5) >= 0) // wait 5ms for a key to be pressed
            break;          // if key was pressed, break the while loop
    }
    return 0; // end program, the camera will be deinitialized automatically in VideoCapture destructor
}
