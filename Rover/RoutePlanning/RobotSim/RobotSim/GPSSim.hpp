#include "autonomous/utils.hpp"
#include <cmath>

namespace RP
{
class GPSSim
{
    public:
        GPSSim();
        RP::point generate_pt(float err, float true_lat, float true_lng);
    private:
        RP::point convertToLatLng(float lat, float lng, float dir, float dist, float angle);
        float convert_to_gps_err(float err_meter, float true_lat, float true_lng);
};
}

