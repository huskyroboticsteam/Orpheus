#pragma once
#include <vector>
#include <utility>

namespace RoverPathfinding
{
    typedef std::pair<float, float> point;
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
	std::vector<std::pair<float, float> > ShortestPathTo(float cur_lat, float cur_lng,
										    float tar_lat, float tar_lng);
    private:
	std::vector<node> build_graph(std::pair<float, float> cur,
				      std::pair<float, float> tar);
	std::vector<obstacle> obstacles;
	std::vector<int> safety_nodes;
    };
    void add_edge(node *n1, node *n2);
    node &create_node(std::vector<node> &nodes, point coord);
    bool segment_circle_intersection(point start, point end, point circle);
    int orientation(point a, point b, point c);
    bool on_segment(point a, point b, point c);
    bool segments_intersect(point p1, point p2, point q1, point q2);
    std::pair<point, point> add_length_to_line_segment(point p, point q, float length);
    float dist_sq(point p1, point p2);
    bool within_radius(point p1, point p2, float R);
}
