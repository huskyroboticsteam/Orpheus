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
	    int prev;
	    float dist_to;
	    std::pair<float, float> coord;
	    std::vector<std::pair<int, float> > connection;
	};

	struct obstacle
	{
	    bool marked;
	    std::pair<float, float> coord1;
	    std::pair<float, float> coord2;
	    std::pair<int, int> side_safety_nodes;
	    int center_safety_node;
	};
    }

    class Map
    {
    public:
	Map() { nodes.resize(2); }
	void AddObstacle(std::pair<float, float> coord1, std::pair<float, float> coord2);
	std::vector<std::pair<float, float> > ShortestPathTo(float cur_lat, float cur_lng,
							     float tar_lat, float tar_lng);
    private:
	std::vector<node> build_graph(std::pair<float, float> cur,
				      std::pair<float, float> tar);
	void add_edge(int n1, int n2);
	int create_node(point coord);
	bool segment_circle_intersection(point start, point end, point circle);
	int orientation(point p, point q, point r);
	bool on_segment(point p, point q, point r);
	bool segments_intersect(point p1, point p2, point q1, point q2);
	point lat_long_offset(float lat1, float lon1, float brng, float dist);
	std::pair<point, point> add_length_to_line_segment(point p, point q, float length);
	point center_point_with_radius(point cur, point p, point q, float R);
	float dist_sq(point p1, point p2);
	bool within_radius(point p1, point p2, float R);

	std::vector<node> nodes;
	std::vector<obstacle> obstacles;
    };
}
