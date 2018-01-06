#include "Map.h"
#include <queue>
#include <cmath>

void RoverPathFinding::Map::AddObstacle(float lat, float lng)
{
}

std::pair<float, float> lat_long_offset(float lat, float lng, float dx, float dy)
{
    #define PI 3.14159265359
    #define R_EARTH 
    #undef PI
}

bool segment_circle_intersection(std::pair<float, float> start,
						    std::pair<float, float> end,
						    std::pair<float, float> circle,
						    float R)
{
    auto direction = std::make_pair(end.first - start.first,
				    end.second - start.second);
    auto center_to_start = std::make_pair(start.first - circle.first,
					  start.second - circle.second);
    float a = direction.first * direction.first + direction.second * direction.second;
    float b = 2 * (center_to_start.first * direction.first + center_to_start.second * direction.second);
    float c = (center_to_start.first * center_to_start.first + center_to_start.second * center_to_start.second) + r * r;

    float descriminant = b * b - 4 * a * c;
    if(descriminant < 0)
    {
	return(std::make_pair(INFINITY, INFINITY));
    }

    descriminant = sqrt(descriminant);
    float t1 = (-b + descriminant) / (2 * a);
    float t2 = (-b - descriminant) / (2 * a);

    return((0 <= t1 && t1 <= 1.0f) || (0 <= t2 && t2 <= 1.0f));	
}

std::vector<std::pair<float, float> > RoverPathFinding::Map::ShortestPathTo(float cur_lat, float cur_long,
									    float tar_lat, float tar_long)
{
    std::vector<edge> edges;
    std::vector<std::pair<float, float> > nodes;
    for(auto obst : obstacles)
    {
	
    }
}
