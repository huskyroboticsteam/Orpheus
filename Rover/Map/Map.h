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
	Map() { nodes.resize(2); } //Allocates space for initial and target node
	void AddObstacle(std::pair<float, float> coord1, std::pair<float, float> coord2); //Adds an obstacle to the map. Obstacle is specified with 2 points
	std::vector<std::pair<float, float> > ShortestPathTo(float cur_lat, float cur_lng,
							     float tar_lat, float tar_lng); //Returns a std::vector of lat/lng pairs that specifies the shortest path to the target destination
    private:
	point lat_long_offset(float lat1, float lon1, float brng, float dist); //Offsets a point with coordinates lat1, lon1, dist meters with bearing brng (0.0 is N)
	bool segment_intersects_circle(point start, point end, point circle, float R); //Unused. Tells whether a line segment intersects with a circle of radius R with center at point "circle"
	int orientation(point p, point q, point r); //Takes three points. //Returns 0 if p, q, and r are colinear, 1 if pq, qr, and rp are clockwise, 2 if pq, qr, and rp are counterclockwise
	bool on_segment(point p, point q, point r); //Tells if three points are on a line segment 
	bool segments_intersect(point p1, point p2, point q1, point q2); //Returns true if segment p1q1 intersects p2q2
	point intersection(point A, point B, point C, point D); //Returns the point of intersection between lines AB and CD
	std::pair<point, point> add_length_to_line_segment(point p, point q, float length); //Returns a pair of points that are "length" away from the ends of segment pq
	point center_point_with_radius(point cur, point p, point q, float R); //Returns a point that is in the middle of pq and is R in the direction of cur
	float dist_sq(point p1, point p2); //Returns the square of the distance between two points
	bool within_radius(point p1, point p2, float R); //Returns whether p1 and p2 are within R of each other
	void add_edge(int n1, int n2); //Adds an edge to the graph
	int create_node(point coord); //Creates a node. Returns index in nodes of the created node
	std::vector<node> build_graph(std::pair<float, float> cur, std::pair<float, float> tar); //Builds the graph using the obstacles so that the shortest path gets calculated	

	std::vector<node> nodes; //The nodes to the graph
	std::vector<obstacle> obstacles; //The obstacles
    };
}
