#ifndef ROBOT_EKF_H
#define ROBOT_EKF_H

#include "kalman/ekfilter.hpp"

#define STATE_VEC_SIZE 4
#define INPUT_VEC_SIZE 2
#define PNOISE_VEC_SIZE 4
#define MEASURE_VEC_SIZE 4
#define MNOISE_VEC_SIZE 4

#define GPS_VARIANCE 3.6
#define ACCELEROMETER_VARIANCE 0.1

class RobotEKF : public Kalman::EKFilter<float, 1> {
	public:
		RobotEKF();
		void setP0(Kalman::KMatrix<float, 1, true>& P);
	protected:
		void makeA();
        void makeH();
        void makeV();
        void makeR();
        void makeW();
        void makeQ();
        void makeProcess();
        void makeMeasure();
		
		float deltaT;
};

#endif