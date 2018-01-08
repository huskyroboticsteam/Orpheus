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
bool RoverPathfinding::segment_circle_intersection(point start,
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

int RoverPathfinding::orientation(point a, point b, point c)
{
    float v = (b.second - a.second) * (c.first - b.first) -
	(b.first - a.first) * (c.second - b.second);
    if(-1e-7 <= v && v <= 1e-7)
	return 0;

    return((v > 0.0f) ? 1 : 2);
}

bool RoverPathfinding::on_segment(point a, point b, point c)
{
    return(a.first <= std::max(b.first, c.first) && b.first >= std::min(a.first, c.first) &&
	   b.second <= std::max(a.second, c.second) && b.second >= std::min(a.second, c.second));
}

bool RoverPathfinding::segments_intersect(point p1, point p2, point q1, point q2)
{
    int o1 = orientation(p1, q1, p2);
    int o2 = orientation(p1, q1, q2);
    int o3 = orientation(p2, q2, p1);
    int o4 = orientation(p2, q2, q1);
 
    if (o1 != o2 && o3 != o4)
        return true;
 
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

std::pair<RoverPathfinding::point, RoverPathfinding::point> RoverPathfinding::add_length_to_line_segment(point p, point q, float length)
{
    point pq = std::make_pair(q.first - p.first, q.second - p.second); //vector
    float len = sqrt(pq.first * pq.first + pq.second * pq.second);
    pq.first = length * pq.first / len;
    pq.second = length * pq.second / len;

    point p1 = std::make_pair(q.first + pq.first, q.second + pq.second);
    point p2 = std::make_pair(p.first - pq.first, p.second - pq.second);
    return(std::make_pair(p1, p2));   
}

float RoverPathfinding::dist_sq(point p1, point p2)
{
    return((p1.first - p2.first) * (p1.first - p2.first) + (p1.second - p2.second) * (p1.second - p2.second));
}

bool RoverPathfinding::within_radius(point p1, point p2, float R)
{
    return(dist_sq(p1, p2) <= R * R);
}

void RoverPathfinding::add_edge(node *n1, node *n2)
{
    auto edge = std::make_pair(n2->id, dist_sq(n1->coord, n2->coord));
    n1->connection.push_back(edge);
    edge.first = n2->id;
    n2->connection.push_back(edge);
}

RoverPathfinding::node &RoverPathfinding::create_node(std::vector<node> &nodes, point coord)
{
    node n;
    n.id = nodes.size();
    n.prev = -1;
    n.dist_to = INFINITY;
    n.coord = coord;
    nodes.push_back(n);
    return(nodes[n.id]);
}

std::vector<RoverPathfinding::node> RoverPathfinding::Map::build_graph(point cur, point tar)
{
    std::vector<node> nodes;
    std::queue<node *> q;
    
    node start;
    start.id = 0;
    start.prev = -1;
    start.dist_to = 0.0f;
    start.coord = cur;
    nodes.push_back(start);
    q.push(&nodes[0]);

    node end;
    end.id = 1;
    end.prev = -1;
    end.dist_to = INFINITY;
    end.coord = tar;
    nodes.push_back(end);
    
    while(!q.empty())
    {
	node *curr_node = q.front();
	q.pop();
	for(auto obst : obstacles)
	{
	    if(segments_intersect(curr_node->coord, tar, obst.coord1, obst.coord2))
	    {
		//TODO(sasha): make R a constant - the following few lines are just a hack
		//             to get R to be in lat/lng units
		auto offset = lat_long_offset(curr_node->coord.first, curr_node->coord.second, 0.5f, 0.0f);
		auto diff = std::make_pair(offset.first - curr_node->coord.first, offset.second - curr_node->coord.second);
		float R = sqrt(diff.first * diff.first + diff.second * diff.second);

		node *n1, *n2;
		//</hack>
		if(!obst.marked)
		{
		    obst.marked = true;
		    auto new_points = add_length_to_line_segment(obst.coord1, obst.coord2, R);
		    
		    bool create_n1 = true;
		    bool create_n2 = true;

		    //TODO(sasha): if this is too slow, use a partitioning scheme to only check against
		    //             nodes in the vicinity
		    for(int safety : this->safety_nodes)
		    {
			node &n = nodes[safety];
			if(within_radius(n.coord, new_points.first, R))
			{
			    create_n1 = false;
			    n1 = &n;
			}
			if(within_radius(n.coord, new_points.second, R))
			{
			    create_n2 = false;
			    n2 = &n;
			}
		    }

		    if(create_n1)
		    {
			n1 = &create_node(nodes, new_points.first);
			this->safety_nodes.push_back(n1->id);
		    }
		    add_edge(curr_node, n1);

		    if(create_n2)
		    {
			n2 = &create_node(nodes, new_points.second);
			this->safety_nodes.push_back(n2->id);
		    }
		    add_edge(curr_node, n2);

		    obst.safety_nodes = std::make_pair(n1->id, n2->id);
		}
		else
		{
		    n1 = &nodes[obst.safety_nodes.first];
		    n2 = &nodes[obst.safety_nodes.second];
		    add_edge(curr_node, n1);
		    add_edge(curr_node, n2);
		}
		q.push(n1);
		q.push(n2);		    
	    }
	    else
	    {
		add_edge(curr_node, &nodes[1]);
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
    auto cmp = [](node *l, node *r) { return l->dist_to < r->dist_to; };
    std::priority_queue<node *, std::vector<node *>, decltype(cmp)> q(cmp);
    q.push(&nodes[0]);
    while(!q.empty())
    {
	node *n = q.top();
	q.pop();

	for(auto &edge : n->connection)
	{
	    float dist = n->dist_to + edge.second;
	    if(dist < nodes[edge.first].dist_to)
	    {
		edge.second = dist;
		nodes[edge.first].prev = n->id;
		q.push(&nodes[edge.first]);
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
