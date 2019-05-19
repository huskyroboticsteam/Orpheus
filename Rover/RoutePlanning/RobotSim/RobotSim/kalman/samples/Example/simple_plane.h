#ifndef PLANE_H
#define PLANE_H

#include "kalman/ekfilter.hpp"


class cPlaneEKF : public Kalman::EKFilter<double,1> {
public:
	cPlaneEKF();

protected:

	void makeA();
	void makeH();
	void makeV();
	void makeR();
	void makeW();
	void makeQ();
	void makeProcess();
	void makeMeasure();

	double Period, Mass, Bfriction, Portance, Gravity;
};

typedef cPlaneEKF::Vector Vector;
typedef cPlaneEKF::Matrix Matrix;

#endif
