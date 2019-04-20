#include <iostream>
#include <math.h> 
using namespace std;

int main()
{
	// recieve data in form of "lat2 long2"
	// "target 47.653469 -122.307556"
	
	bool pointed = false;
	
	
	//recieve current magmatometer and current GPS location
	// "47.653469 -122.307556 125.7"
		
		
		
	
	
	//cout << "Enter Current Heading" << endl;
	double currentHeading = 0.0; //whatever the magnotometer data is
	//cin >> currentHeading;
	
	//cout << "Enter Desired Heading" << endl;
	double desiredHeading = 0.0; //calcuated based on current location and desired location
	//cin >> desiredHeading;
	
	double difference = desiredHeading - currentHeading;
	if (difference < -180){
		difference = 360 + difference;
	} else if (difference > 180){
		difference = difference - 360;
	}
	
	cout << difference << endl;
	
	//turn rover to desired direction
	
	
	//demo
	//double lat1 = 1.0;
	//double long1 = 1.0;
	
	//near fountaun
	double lat1 = 47.653469;
	double long1 = -122.307556;
	
	//ver close to stevens way
	//double lat1 = 47.652145;
	//double long1 = -122.306905;
	
	//near stevens way
	double lat2 = 47.652131;
	double long2 = -122.306881;
	
	double distance = sqrt( pow(long2 - long1, 2) + pow(lat2 - lat1, 2)); 
	cout << "distances = " << distance *1000 << endl;
		
}