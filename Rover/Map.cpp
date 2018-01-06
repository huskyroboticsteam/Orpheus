#include "Map.h"
#include <queue>
#include <cmath>

void RoverPathFinding::Map::AddObstacle(float lat, float lng)
{
    obstacle o;
    o.marked = false;
    o.coord = std::make_pair(lat, lng);    
    obstacles.push_back(o);
}

std::pair<float, float> lat_long_offset(float lat, float lng, float dx, float dy)
{
#define PI 3.14159265359
#define R_EARTH 
#undef PI
}


//start, end, circle, and R are in lat/long coordinates
bool segment_circle_intersection(std::pair<float, float> start,
				 std::pair<float, float> end,
				 std::pair<float, float> circle)
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
    float c = (center_to_start.first * center_to_start.first + center_to_start.second * center_to_start.second) + r * r;

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

std::vector<node> build_graph(std::pair<float, float> cur,
			      std::pair<float, float> tar)
{
    std::vector<node> nodes;
    std::queue<node &> q;
    
    node start;
    start.id = 0;
    start.coord = cur;
    nodes.push_back(start);
    q.push(nodes[0]);
    
    while(!q.empty())
    {
	node &curr_node = q.front;
	q.pop();
	for(auto obst : obstacles)
	{
	    if(!obst.marked)
	    {
		obst.marked = true;
		if(segment_circle_intersection(curr_node.coord, tar, obst))
		{
#define NUM_SAFE_POINTS 6
#define SAFE_RADIUS 0.55f
#define PI 3.14159265359
		    float angle = 0.0f;
		    for(int i = 0; i < NUM_SAFE_POINTS; i++)
		    {
			node n;
			n.id = nodes.size();
			n.coord = lat_long_offset(obst.first, obst.second,
						  0.55f * cos(angle), 0.55f * sin(angle));
			nodes.push_back(n);
			edge e;
			e.from = curr_node.id;
			e.to = n.id;
			e.length = (curr_node.coord.first - n.coord.first) * (curr_node.coord.first - n.coord.first) +
			    (curr_node.coord.second - n.coord.second) * (curr_node.coord.second - n.coord.second);
			curr_node.connection.push_back(e);
			std::swap(e.from, e.to);
			n.connection.push_back(e);
			angle += PI / NUM_SAFE_POINTS);
		    }
#undef PI
#undef SAFE_RADIUS
#undef NUM_SAFE_POINTS
		}
	    }
	    return(nodes);
	}

    }
}
std::vector<std::pair<float, float> > RoverPathFinding::Map::ShortestPathTo(float cur_lat, float cur_lng,
									    float tar_lat, float tar_lng)
{
    auto cur = std::make_pair(cur_lat, cur_lng);
    auto tar = std::make_pair(tar_lat, tar_lng);
    std::vector<node> nodes = build_graph(cur, tar);

}
