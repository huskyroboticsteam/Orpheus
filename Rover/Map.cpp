#include "Map.h"
#include <queue>
#include <cmath>

void RoverPathFinding::Map::AddObstacle(float lat, float lng)
{
}

static std::pair<float, float> lat_long_offset(float lat, float lng, float dx, float dy)
{
    #define PI 3.14159265359
    #define R_EARTH 
    #undef PI    
}

std::vector<std::pair<float, float> > RoverPathFinding::Map::ShortestPathTo(float cur_lat, float cur_long,
									    float tar_lat, float tar_long)
{
    std::vector<edge> edges;
    std::vector<std::pair<float, float> > nodes;
    for(auto obst : obstacles)
    {
	
    }
//    std::priority_queue<
}
