#pragma once
#include <vector>
#include <utility>

namespace RoverPathfinding
{
    namespace
    {
	struct edge
	{
	    float to;
	    float from;
	    float distance;
	};
    }

    class Map
    {
    public:
	void AddObstacle(float lat, float longi);
    private:
	std::vector<std::pair<float, float> > obstacles;
    };
}
