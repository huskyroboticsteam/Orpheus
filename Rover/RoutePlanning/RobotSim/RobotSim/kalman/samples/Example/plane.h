#ifndef PLANE_H
#define PLANE_H

#include "kalman/ekfilter.hpp"


class cPlaneEKF : public Kalman::EKFilter<double,1,false,true,false> {
public:
	cPlaneEKF();

protected:
	void makeBaseA();
	void makeBaseH();
	void makeBaseV();
	void makeBaseR();
	void makeBaseW();
	void makeBaseQ();

	void makeA();
	void makeH();
	void makeProcess();
	void makeMeasure();

	double Period, Mass, Bfriction, Portance, Gravity;
};

typedef cPlaneEKF::Vector Vector;
typedef cPlaneEKF::Matrix Matrix;

#endif
