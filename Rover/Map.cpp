#include <vector>

namespace RoverPathfinding
{
    namespace
    {
	struct edge
	{
	    float to;
	    float from;
	};
    }

    class Map
    {
    public:
	
    private:
	std::vector<edge> edges;
    };
}
