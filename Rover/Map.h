#pragma once
#include <vector>
#include <utility>

namespace RoverPathfinding
{
    namespace
    {
	struct node
	{
	    int id;
	    std::pair<float, float> coord;
	    std::vector<edge> connection;
	};

	struct edge
	{
	    int from;
	    int to;
	    float length;
	};

	struct obstacle
	{
	    bool marked;
	    std::pair<float, float> coord;
	    std::vector<int> safety_nodes;
	};
    }

    class Map
    {
    public:
	void AddObstacle(float lat, float lng);
    private:
	std::vector<node> build_graph(std::pair<float, float> cur,
				      std::pair<float, float> tar);	   
	std::vector<std::pair<float, float> > obstacles;
    };
}
