#include <iostream>
#include <opencv2/core.hpp>
#include <opencv2/dnn.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/imgcodecs.hpp>
#include <opencv2/imgproc.hpp>
#include "detector.hpp"

using namespace std;
using namespace cv;

int main(int argc, char** argv) {
    std::string graphPath = "./data/final_models/frozen_inference_graph.pb";
    std::string graphPbtxt = "./data/final_models/graph.pbtxt";

    tb::Detector det (graphPath, graphPbtxt);

    string imagePath = "./images/1.jpg";
	
	if(argc > 1){
	  imagePath = argv[1];
	}

    Mat frame = imread(imagePath);

    vector<tb::Detection> detections = det.performDetection(frame);
    int i = 0;
    for(tb::Detection current : detections){
        rectangle(frame, current.getBBoxRect(), Scalar(0, 255, 0), 2);
        ostringstream output;
        output << i << " - ";
        output << "bbox: [" << current.getBBoxLeft() << ", " << current.getBBoxRight() << "]";
        output << " x [" << current.getBBoxTop() << ", " << current.getBBoxBottom() << "] ";
        output << "conf: " << current.getConfidencePct() << "%";
        cout << output.str() << endl;
        i++;
    }
    imshow("image", frame);
    waitKey(0);
}
