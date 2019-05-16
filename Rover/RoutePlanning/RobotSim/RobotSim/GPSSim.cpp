#include "GPSSim.hpp"
#include <iostream>

#define CONV_FACTOR_LNG 8.627
#define DEGREES_METER_LNG 0.0001
#define CONV_FACTOR_LAT 111319.9

RP::GPSSim::GPSSim() {
}

// err is in meters, true_lat and true_lng are gps coordinates
RP::point RP::GPSSim::generate_pt(float err_margin, float true_lat, float true_lng) {
    float err = convert_to_gps_err(err_margin, true_lat, true_lng) * ((double) rand() / (RAND_MAX) - 0.5);
    RP::point p = {true_lat + err, true_lng - err};
    return p;
}

float RP::GPSSim::convert_to_gps_err(float err_meter, float true_lat, float true_lng) {
    RP::point pt = convertToLatLng(0.0, 0.0, 0.0, err_meter, 0.0);
    return pt.x;
}

RP::point RP::GPSSim::convertToLatLng(float lat, float lng, float dir, float dist, float angle) {
		float delta_x = dist * cos(angle + dir + M_PI/2);
		float delta_y = dist * sin(angle + dir + M_PI/2);
		float delta_lng = delta_x / CONV_FACTOR_LNG * DEGREES_METER_LNG;
		float delta_lat = delta_y / CONV_FACTOR_LAT;
		RP::point p;
		p.x = delta_lat + lat;
		p.y = delta_lng + lng;
		std::cout << "dist: " << dist << std::endl;
		std::cout << "p.x, p.y: " << p.x << ", " << p.y << std::endl;
		return p;
}

