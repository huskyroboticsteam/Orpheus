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
	    std::vector<std::pair<int, float> > connection;
	};

	struct obstacle
	{
	    bool marked;
	    std::pair<float, float> coord1;
	    std::pair<float, float> coord2;
	    std::pair<int, int> safety_nodes;
	};
    }

    class Map
    {
    public:
	void AddObstacle(std::pair<float, float> coord1, std::pair<float, float> coord2);
    private:
	std::vector<node> build_graph(std::pair<float, float> cur,
				      std::pair<float, float> tar);
	std::vector<std::pair<float, float> > obstacles;
	std::vector<int> safety_nodes;
    };
}
