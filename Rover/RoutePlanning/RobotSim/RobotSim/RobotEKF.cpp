#include "RobotEKF.hpp"
#include <cmath>



RobotEKF::RobotEKF() {
	
	setDim(STATE_VEC_SIZE, INPUT_VEC_SIZE, PNOISE_VEC_SIZE,
	       MEASURE_VEC_SIZE, MNOISE_VEC_SIZE);
	deltaT = 0.2;
	//std::cout << "Constructing EKF" << std::endl;
}

void RobotEKF::makeProcess() {
	//std::cout << "Make process" << std::endl;
	/*Vector x_(x.size());
	float xPos = x(1);
	float yPos = x(2);
	float xVel = x(3);
	float yVel = x(4);
	float xAcc = x(5);
	float yAcc = x(6);
	x_(1) = xPos + deltaT*xVel + (pow(deltaT, 2)/2) * xAcc;
	x_(2) = yPos + deltaT*yVel + (pow(deltaT, 2)/2) * yAcc;
	x_(3) = xVel + deltaT*xAcc;
	x_(4) = yVel + deltaT*yAcc;
	x_(5) = xAcc;
	x_(6) = yAcc;
	x.swap(x_);*/
	Vector x_(x.size());
	float xPos = x(1);
	float yPos = x(2);
	float xVel = x(3);
	float yVel = x(4);
	float xAcc = u(1);
	float yAcc = u(2);
	
	x_(1) = xPos + deltaT*xVel ;//+ (pow(deltaT, 2)/2) * xAcc;
	x_(2) = yPos + deltaT*yVel ;//+ (pow(deltaT, 2)/2) * yAcc;
	x_(3) = xVel ;//+ deltaT*xAcc;
	x_(4) = yVel ;//+ deltaT*yAcc; 
	x.swap(x_);
	
}

void RobotEKF::makeMeasure() {
	z(1) = x(1);
	z(2) = x(2);
}

void RobotEKF::makeA() {
	//std::cout << "Making A" << std::endl;
	/*A(1,1) = 1;
	A(1,2) = 0;
	A(1,3) = deltaT;
	A(1,4) = 0;
	A(1,5) = pow(deltaT, 2);
	A(1,6) = 0;
	
	A(2,1) = 0;
	A(2,2) = 1;
	A(2,3) = 0;
	A(2,4) = deltaT;
	A(2,5) = 0;
	A(2,6) = pow(deltaT, 2);
	
	A(3,1) = 0;
	A(3,2) = 0;
	A(3,3) = 1;
	A(3,4) = 0;
	A(3,5) = deltaT;
	A(3,6) = 0;
	
	A(4,1) = 0;
	A(4,2) = 0;
	A(4,3) = 0;
	A(4,4) = 1;
	A(4,5) = 0;
	A(4,6) = deltaT;
	
	A(5,1) = 0;
	A(5,2) = 0;
	A(5,3) = 0;
	A(5,4) = 0;
	A(5,5) = 1;
	A(5,6) = 0;
	
	A(6,1) = 0;
	A(6,2) = 0;
	A(6,3) = 0;
	A(6,4) = 0;
	A(6,5) = 0;
	A(6,6) = 1;*/
	A(1,1) = 1;
	A(1,2) = 0;
	A(1,3) = deltaT;
	A(1,4) = 0;
	
	A(2,1) = 0;
	A(2,2) = 1;
	A(2,3) = 0;
	A(2,4) = deltaT;
	
	A(3,1) = 0;
	A(3,2) = 0;
	A(3,3) = 1;
	A(3,4) = 0;
	
	A(4,1) = 0;
	A(4,2) = 0;
	A(4,3) = 0;
	A(4,4) = 1;
	
}

void RobotEKF::makeW() {
	/*W(1,1) = 0;
	W(1,2) = 0;
	
	W(2,1) = 0;
	W(2,2) = 0;
	
	W(3,1) = 1;
	W(3,2) = 0;
	
	W(4,1) = 0;
	W(4,2) = 1;
	
	W(5,1) = 0;
	W(5,2) = 0;
	
	W(6,1) = 0;
	W(6,2) = 0;*/
	/*W(1,1) = 1;
	W(1,2) = 0;
	W(1,3) = 0;
	W(1,4) = 0;
	
	W(2,1) = 0;
	W(2,2) = 1;
	W(2,3) = 0;
	W(2,4) = 0;
	
	W(3,1) = 0;
	W(3,2) = 0;
	W(3,3) = 1;
	W(3,4) = 0;
	
	W(4,1) = 0;
	W(4,2) = 0;
	W(4,3) = 0;
	W(4,4) = 1;*/
	//std::cout << "Making W" << std::endl;
	for(int i = 1; i <= STATE_VEC_SIZE; i++) {
		for(int j = 1; j <= PNOISE_VEC_SIZE; j++) {
			W(i,j) = 0;
		}
		
	}
	W(3,1) = 1;
	W(4,2) = 1;
}

void RobotEKF::makeV() {
	/*V(1,1) = 1;
	V(1,2) = 0;
	V(1,3) = 0;
	V(1,4) = 0;
	
	V(2,1) = 0;
	V(2,2) = 1;
	V(2,3) = 0;
	V(2,4) = 0;
	
	V(3,1) = 0;
	V(3,2) = 0;
	V(3,3) = 1;
	V(3,4) = 0;
	
	V(4,1) = 0;
	V(4,2) = 0;
	V(4,3) = 0;
	V(4,4) = 1;*/
	V(1,1) = 1;
	V(1,2) = 0;
	
	V(2,1) = 0;
	V(2,2) = 1;
	//std::cout << "Making V" << std::endl;
	for(int i = 1; i <= MEASURE_VEC_SIZE; i++) {
		for(int j = 1; j <=MNOISE_VEC_SIZE; j++) {
			if(i==j) {
				V(i,j) = 10;
			} else  {
				V(i,j) = 0;
			}
		}
	}
}

void RobotEKF::makeH() {
	/*H(1,1) = 1;
	H(1,2) = 0;
	H(1,3) = 0;
	H(1,4) = 0;
	H(1,5) = 0;
	H(1,6) = 0;
	
	H(2,1) = 0;
	H(2,2) = 1;
	H(2,3) = 0;
	H(2,4) = 0;
	H(2,5) = 0;
	H(2,6) = 0;
		
	H(3,1) = 0;
	H(3,2) = 0;
	H(3,3) = 0;
	H(3,4) = 0;
	H(3,5) = 1;
	H(3,6) = 0;
	
	H(4,1) = 0;
	H(4,2) = 0;
	H(4,3) = 0;
	H(4,4) = 0;
	H(4,5) = 0;
	H(4,6) = 1;*/
	//std::cout << "Making H" << std::endl;
	for(int i = 1; i < MEASURE_VEC_SIZE; i++) {
		for(int j = 1; j < STATE_VEC_SIZE; j++) {
			if(i==j) {
				H(i,j) = 1;
			} else {
				H(i,j) = 0;
			}
		}
	}
	/*H(1,1) = 1;
	H(1,2) = 0;
	H(1,3) = 0;
	H(1,4) = 0;
	
	H(2,1) = 0;
	H(2,2) = 1;
	H(2,3) = 0;
	H(2,4) = 0;*/
	
}

void RobotEKF::makeR() {
	//std::cout << "Making R" << std::endl;
	R(1,1) = GPS_VARIANCE*2.5;
	R(1,2) = 0;
	R(1,3) = 0;
	R(1,4) = 0;
	
	R(2,1) = 0;
	R(2,2) = GPS_VARIANCE*2.5;
	R(2,3) = 0;
	R(2,4) = 0;
	
	R(3,1) = 0;
	R(3,2) = 0;
    R(3,3) = ACCELEROMETER_VARIANCE*15;
	R(3,4) = 0;
	
	R(4,1) = 0;
	R(4,2) = 0;
	R(4,3) = 0;
	R(4,4) = ACCELEROMETER_VARIANCE*15;
	//R(1,1) = 1.8;
	//R(1,2) = 0;
	
	//R(2,1) = 0;
	//R(2,2) = 1.8;
}

/*void RobotEKF::makeP() {
	P(1,1) = 100;
	P(1,2) = 0;
	P(1,3) = 0;
	P(1,4) = 0;
	P(1,5) = 0;
	P(1,6) = 0;
	
	P(2,1) = 0;
	P(2,2) = 100;
	P(2,3) = 0;
	P(2,4) = 0;
	P(2,5) = 0;
	P(2,6) = 0;
	
	P(3,1) = 0;
	P(3,2) = 0;
	P(3,3) = 10;
	P(3,4) = 0;
	P(3,5) = 0;
	P(3,6) = 0;	
	
	P(4,1) = 0;
	P(4,2) = 0;
	P(4,3) = 0;
	P(4,4) = 10;
	P(4,5) = 0;
	P(4,6) = 0;

	P(5,1) = 0;
	P(5,2) = 0;
	P(5,3) = 0;
	P(5,4) = 0;
	P(5,5) = 1;
	P(5,6) = 0;	
	
	P(6,1) = 0;
	P(6,2) = 0;
	P(6,3) = 0;
	P(6,4) = 0;
	P(6,5) = 1;
	P(6,6) = 0;
}*/

void RobotEKF::makeQ() {
	//std::cout << "Making Q" << std::endl;
	for(int i = 1; i <= PNOISE_VEC_SIZE; i++) {
		for(int j = 1; j <= PNOISE_VEC_SIZE; j++) {
			if(i==j) {
				Q(i,j) = 1;
			} else {
			
				Q(i,j) = 0;
			}
		}
	}
}

void RobotEKF::setP0(Kalman::KMatrix<float, 1, true>& P) {
	/*P(1,1) = 100;
	P(1,2) = 0;
	P(1,3) = 0;
	P(1,4) = 0;
	P(1,5) = 0;
	P(1,6) = 0;
	
	P(2,1) = 0;
	P(2,2) = 100;
	P(2,3) = 0;
	P(2,4) = 0;
	P(2,5) = 0;
	P(2,6) = 0;
	
	P(3,1) = 0;
	P(3,2) = 0;
	P(3,3) = 10;
	P(3,4) = 0;
	P(3,5) = 0;
	P(3,6) = 0;	
	
	P(4,1) = 0;
	P(4,2) = 0;
	P(4,3) = 0;
	P(4,4) = 10;
	P(4,5) = 0;
	P(4,6) = 0;

	P(5,1) = 0;
	P(5,2) = 0;
	P(5,3) = 0;
	P(5,4) = 0;
	P(5,5) = 1;
	P(5,6) = 0;	
	
	P(6,1) = 0;
	P(6,2) = 0;
	P(6,3) = 0;
	P(6,4) = 0;
	P(6,5) = 1;
	P(6,6) = 0;*/
	P(1,1) = 10;
	P(1,2) = 0;
	P(1,3) = 0;
	P(1,4) = 0;
	
	P(2,1) = 0;
	P(2,2) = 10;
	P(2,3) = 0;
	P(2,4) = 0;
	
	P(3,1) = 0;
	P(3,2) = 0;
	P(3,3) = 1;
	P(3,4) = 0;
	
	P(4,1) = 0;
	P(4,2) = 0;
	P(4,3) = 0;
	P(4,4) = 1;
	
}