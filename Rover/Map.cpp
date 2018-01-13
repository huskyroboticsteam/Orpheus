#include "Map.h"
#include <queue>
#include <cmath>
#include <algorithm>

#define R_EARTH 6371.0088 // in km
#define PI 3.14159265359

void RoverPathfinding::Map::AddObstacle(point coord1, point coord2)
{
    obstacle o;
    o.marked = false;
    o.coord1 = coord1;
    o.coord2 = coord2;
    obstacles.push_back(o);
}


float deg_to_rad(float deg)
{
    return(deg * PI / 180.0f);
}

float rad_to_deg(float rad)
{
    return(rad * 180.0f / PI);
}

// given lat, long, bearing (in degrees), and distance (in km), returns a new point
RoverPathfinding::point RoverPathfinding::Map::lat_long_offset(float lat1, float lon1, float brng, float dist)
{
    dist /= 1000.0f;

    lat1 = deg_to_rad(lat1);
    lon1 = deg_to_rad(lon1);
    brng = deg_to_rad(brng);
    float lat2 = asin(sin(lat1) * cos(dist / R_EARTH) + cos(lat1) * sin(dist / R_EARTH) * cos(brng));
    float lon2 = lon1 + atan2(sin(brng) * sin(dist / R_EARTH) * cos(lat1), cos(dist / R_EARTH) - sin(lat1) * sin(lat2));
    lat2 = rad_to_deg(lat2);
    lon2 = rad_to_deg(lon2);
    return(std::make_pair(lat2, lon2));
}

//start, end, circle, and R are in lat/long coordinates
bool RoverPathfinding::Map::segment_intersects_circle(point start,
						      point end,
						      point circle,
						      float R)
{

    auto direction = std::make_pair(end.first - start.first,
				    end.second - start.second);
    auto center_to_start = std::make_pair(start.first - circle.first,
					  start.second - circle.second);
    float a = direction.first * direction.first + direction.second * direction.second;
    float b = 2 * (center_to_start.first * direction.first + center_to_start.second * direction.second);
    float c = (center_to_start.first * center_to_start.first + center_to_start.second * center_to_start.second) + R * R;

    float descriminant = b * b - 4 * a * c;
    if(descriminant < 0)
    {
	return(false);
    }

    descriminant = sqrt(descriminant);
    float t1 = (-b + descriminant) / (2 * a);
    float t2 = (-b - descriminant) / (2 * a);

    return((0 <= t1 && t1 <= 1.0f) || (0 <= t2 && t2 <= 1.0f));	
}

#define COLINEAR 0
#define CLOCKWISE 1
#define COUNTERCLOCKWISE 2
//Returns 0 if p, q, and r are colinear.
//Returns 1 if pq, qr, and rp are clockwise
//Returns 2 if pq, qr, and rp are counterclockwise
int RoverPathfinding::Map::orientation(point p, point q, point r)
{
    float v = (q.second - p.second) * (r.first - q.first) -
	(q.first - p.first) * (r.second - q.second);
    if(-1e-7 <= v && v <= 1e-7)
	return COLINEAR;

    return((v > 0.0f) ? CLOCKWISE : COUNTERCLOCKWISE);
}

//Tells if r is on segment pq
bool RoverPathfinding::Map::on_segment(point p, point q, point r)
{
    return(q.first <= std::max(p.first, r.first) && q.first >= std::min(p.first, r.first) &&
	   q.second <= std::max(p.second, r.second) && q.second >= std::min(p.second, r.second));
}

bool RoverPathfinding::Map::segments_intersect(point p1, point p2, point q1, point q2)
{
    int o1 = orientation(p1, q1, p2);
    int o2 = orientation(p1, q1, q2);
    int o3 = orientation(p2, q2, p1);
    int o4 = orientation(p2, q2, q1);
 
    if (o1 != o2 && o3 != o4)
        return(true);
 
    if (o1 == 0 && on_segment(p1, p2, q1))
	return(true);
    
    if (o2 == 0 && on_segment(p1, q2, q1))
	return(true);
 
    if (o3 == 0 && on_segment(p2, p1, q2))
	return(true);
 
    if (o4 == 0 && on_segment(p2, q1, q2))
	return(true);
 
    return(false);
}

RoverPathfinding::point RoverPathfinding::Map::intersection(RoverPathfinding::point A, RoverPathfinding::point B, RoverPathfinding::point C, RoverPathfinding::point D)
{
    // Line AB represented as a1x + b1y = c1
    float a1 = B.second - A.second;
    float b1 = A.first - B.first;
    float c1 = a1*(A.first) + b1*(A.second);
 
    // Line CD represented as a2x + b2y = c2
    float a2 = D.second - C.second;
    float b2 = C.first - D.first;
    float c2 = a2*(C.first)+ b2*(C.second);
 
    float determinant = a1*b2 - a2*b1;

    // The lines are parallel. This is simplified
    // by returning a pair of FLT_MAX
    if (-1e-7 <= determinant && determinant <= 1e-7)
        return(std::make_pair(INFINITY, INFINITY));
    
    float x = (b2*c1 - b1*c2)/determinant;
    float y = (a1*c2 - a2*c1)/determinant;
    return(std::make_pair(x, y));
}

std::pair<RoverPathfinding::point, RoverPathfinding::point> RoverPathfinding::Map::add_length_to_line_segment(point p, point q, float length)
{
    point pq = std::make_pair(q.first - p.first, q.second - p.second); //vector
    float pq_len = sqrt(pq.first * pq.first + pq.second * pq.second);
    pq.first = length * pq.first / pq_len;
    pq.second = length * pq.second / pq_len;

    point p1 = std::make_pair(q.first + pq.first, q.second + pq.second);
    point p2 = std::make_pair(p.first - pq.first, p.second - pq.second);
    return(std::make_pair(p1, p2));   
}

//Returns a point in the center of segment pq and then moves it R towards cur
RoverPathfinding::point RoverPathfinding::Map::center_point_with_radius(RoverPathfinding::point cur, RoverPathfinding::point p, RoverPathfinding::point q, float R)
{
    point vec = std::make_pair(-p.second + q.second, p.first - q.first);
    float len = sqrt((vec.first * vec.first) + (vec.second * vec.second));
    vec.first = vec.first * R / len;
    vec.second = vec.second * R / len;
    point result = std::make_pair((p.first + q.first) / 2.0f, (p.second + q.second) / 2.0f);
    int o = orientation(cur, p, q);
    if(o == CLOCKWISE)
    {
	result.first += vec.first;
	result.second += vec.second;
    }
    else //o is COUNTERCLOCKWISE
    {
	result.first -= vec.first;
	result.second -= vec.second;
    }
    return(result);
}

float RoverPathfinding::Map::dist_sq(point p1, point p2)
{
    return((p1.first - p2.first) * (p1.first - p2.first) + (p1.second - p2.second) * (p1.second - p2.second));
}

bool RoverPathfinding::Map::within_radius(point p1, point p2, float R)
{
    return(dist_sq(p1, p2) <= R * R);
}

void RoverPathfinding::Map::add_edge(int n1, int n2)
{
    float dist = sqrt(dist_sq(nodes[n1].coord, nodes[n2].coord));
    
    auto n1_to_n2 = std::make_pair(n2, dist);
    nodes[n1].connection.push_back(n1_to_n2);

    auto n2_to_n1 = std::make_pair(n1, dist);
    nodes[n2].connection.push_back(n2_to_n1);   
}

int RoverPathfinding::Map::create_node(point coord)
{
    node n;
    n.prev = -1;
    n.dist_to = INFINITY;
    n.coord = coord;
    nodes.push_back(n);
    return(nodes.size() - 1);
}

std::vector<RoverPathfinding::node> RoverPathfinding::Map::build_graph(point cur, point tar)
{
    //TODO(sasha): make R a constant - the following few lines are just a hack
    //             to get R to be in lat/lng units
#define R_METERS 0.5f
    auto offset = lat_long_offset(cur.first, cur.second, 0.0f, R_METERS);
    auto diff = std::make_pair(offset.first - cur.first, offset.second - cur.second);
    float R = sqrt(diff.first * diff.first + diff.second * diff.second);
#undef R_METERS
    //</hack>		
    
    node start;
    for(auto &n : nodes)
    {
	n.dist_to = INFINITY;
	n.connection.clear();
    }

    nodes[0].prev = -1;
    nodes[0].dist_to = 0.0f;
    nodes[0].coord = cur;

    nodes[1].coord = tar;

    if(obstacles.empty())
    {
	add_edge(0, 1);
	return(nodes);
    }

    std::queue<int> unprocessed_nodes;
    unprocessed_nodes.push(0);
    while(!unprocessed_nodes.empty())
    {
	int curr_node = unprocessed_nodes.front();
	unprocessed_nodes.pop();
	bool destination_blocked = false;
	int closest_obst;
	float min_dist = INFINITY;
	for(int i = 0; i < obstacles.size(); i++)
	{
	    auto &obst = obstacles[i];
	    if(segments_intersect(nodes[curr_node].coord, obst.coord1, tar, obst.coord2))
	    {
		destination_blocked = true;
		point inters = intersection(nodes[curr_node].coord, tar, obst.coord1, obst.coord2);
		float dist = dist_sq(nodes[curr_node].coord, inters);
		if(dist < min_dist)
		{
		    min_dist = dist;
		    closest_obst = i;
		}
	    }	    
	}
	if(destination_blocked)
	{
	    auto &obst = obstacles[closest_obst];
	    int n1, n2; 
	    if(!obst.marked)
	    {
		obst.marked = true;
		auto new_points = add_length_to_line_segment(obst.coord1, obst.coord2, R);
		    
		bool create_n1 = true;
		bool create_n2 = true;

		//TODO(sasha): if this is too slow, use a partitioning scheme to only check against
		//             nodes in the vicinity
		for(int safety = 2; safety < nodes.size(); safety++)
		{
		    node &n = nodes[safety];
		    if(within_radius(n.coord, new_points.first, R))
		    {
			create_n1 = false;
			n1 = safety;
		    }
		    if(within_radius(n.coord, new_points.second, R))
		    {
			create_n2 = false;
			n2 = safety;
		    }
		}

		if(create_n1)
		    n1 = create_node(new_points.first);

		if(create_n2)
		    n2 = create_node(new_points.second);		   
		    
		obst.side_safety_nodes = std::make_pair(n1, n2);

		point center_coord = center_point_with_radius(nodes[curr_node].coord, new_points.first, new_points.second, R);
		obst.center_safety_node = create_node(center_coord);
		add_edge(curr_node, obst.center_safety_node);
		add_edge(n1, obst.center_safety_node);
		add_edge(n2, obst.center_safety_node);
	    }
	    else
	    {
		n1 = obst.side_safety_nodes.first;
		n2 = obst.side_safety_nodes.second;
	    }

	    add_edge(curr_node, n1);
	    add_edge(curr_node, n2);

	    unprocessed_nodes.push(n1);
	    unprocessed_nodes.push(n2);
	}
	else
	{
	    add_edge(curr_node, 1);
	}		    
    }
    return(nodes);
}

//TODO(sasha): Find heuristics and upgrade to A*
std::vector<std::pair<float, float> > RoverPathfinding::Map::ShortestPathTo(float cur_lat, float cur_lng,
									    float tar_lat, float tar_lng)
{
    auto cur = std::make_pair(cur_lat, cur_lng);
    auto tar = std::make_pair(tar_lat, tar_lng);
    std::vector<node> nodes = build_graph(cur, tar);

#if 0
    for(int i = 0; i < nodes.size(); i++)
    {
	std::cout << "Node " << i << " at (" << nodes[i].coord.first << ", " << nodes[i].coord.second << ") connected to: " << std::endl << '\t';
	for(auto con : nodes[i].connection)
	    std::cout << con.first << ' ';
	std::cout << std::endl;
    }
#endif
    
    auto cmp = [nodes](int l, int r) { return nodes[l].dist_to < nodes[r].dist_to; };
    std::priority_queue<int, std::vector<int>, decltype(cmp)> q(cmp);
    q.push(0);
    while(!q.empty())
    {
	int n = q.top();
	q.pop();

	for(auto &edge : nodes[n].connection)
	{
	    float dist = nodes[n].dist_to + edge.second;
	    if(dist < nodes[edge.first].dist_to)
	    {
		nodes[edge.first].prev = n;
		nodes[edge.first].dist_to = dist;
		q.push(edge.first);
	    }
	}
    }
    
    std::vector<std::pair<float, float> > result;
    int i = 1;
    while(i != 0)
    {
	node &n = nodes[i];
	result.push_back(n.coord);
	i = n.prev;
    }
    std::reverse(result.begin(), result.end());
    return(result);
}
