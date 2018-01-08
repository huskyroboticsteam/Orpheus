#include "Map.h"
#include <queue>
#include <cmath>
#include <algorithm>

void RoverPathfinding::Map::AddObstacle(point coord1, point coord2)
{
    obstacle o;
    o.marked = false;
    o.coord1 = coord1;
    o.coord2 = coord2;
    obstacles.push_back(o);
}

std::pair<float, float> lat_long_offset(float lat, float lng, float dx, float dy)
{
#define PI 3.14159265359
#define R_EARTH 
#undef PI
    return(std::make_pair(lat + dx, lat + dy));
}


//start, end, circle, and R are in lat/long coordinates
bool RoverPathfinding::Map::segment_circle_intersection(point start,
						   point end,
						   point circle)
{

    //TODO(sasha): make R a constant - the following few lines are just a hack
    //             to get R to be in lat/lng units
    auto offset = lat_long_offset(circle.first, circle.second, 0.5f, 0.0f);
    auto diff = std::make_pair(offset.first - circle.first, offset.second - circle.second);
    float R = sqrt(diff.first * diff.first + diff.second * diff.second);
    //</hack>
    
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

int RoverPathfinding::Map::orientation(point p, point q, point r)
{
    float v = (q.second - p.second) * (r.first - q.first) -
	(q.first - p.first) * (r.second - q.second);
    if(-1e-7 <= v && v <= 1e-7)
	return 0;

    return((v > 0.0f) ? 1 : 2);
}

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

std::pair<RoverPathfinding::point, RoverPathfinding::point> RoverPathfinding::Map::add_length_to_line_segment(point p, point q, float length)
{
    point pq = std::make_pair(q.first - p.first, q.second - p.second); //vector
    float len = sqrt(pq.first * pq.first + pq.second * pq.second);
    pq.first = length * pq.first / len;
    pq.second = length * pq.second / len;

    point p1 = std::make_pair(q.first + pq.first, q.second + pq.second);
    point p2 = std::make_pair(p.first - pq.first, p.second - pq.second);
    return(std::make_pair(p1, p2));   
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
    float dist = dist_sq(nodes[n1].coord, nodes[n2].coord);
    
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
    std::queue<int> q;
    
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

    q.push(0);
    while(!q.empty())
    {
	int curr_node = q.front();
	q.pop();
	for(auto obst : obstacles)
	{
	    if(segments_intersect(nodes[curr_node].coord, obst.coord1, tar, obst.coord2))
	    {
		//TODO(sasha): make R a constant - the following few lines are just a hack
		//             to get R to be in lat/lng units
		auto offset = lat_long_offset(nodes[curr_node].coord.first, nodes[curr_node].coord.second, 0.5f, 0.0f);
		auto diff = std::make_pair(offset.first - nodes[curr_node].coord.first, offset.second - nodes[curr_node].coord.second);
		float R = sqrt(diff.first * diff.first + diff.second * diff.second);
		//</hack>
		
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
		    add_edge(curr_node, n1);

		    if(create_n2)
			n2 = create_node(new_points.second);
		    
		    add_edge(curr_node, n2);
		    
		    obst.safety_nodes = std::make_pair(n1, n2);
		}
		else
		{
		    n1 = obst.safety_nodes.first;
		    n2 = obst.safety_nodes.second;
		    add_edge(curr_node, n1);
		    add_edge(curr_node, n2);
		}
		
		q.push(n1);
		q.push(n2);
	    }
	    else
	    {
		add_edge(curr_node, 1);
	    }
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
