#include <iostream>
#include "Map.h"
#include "Map.cpp"

int main(void)
{
    RoverPathfinding::Map m;
    m.AddObstacle(std::make_pair(-4.0f, 5.0f), std::make_pair(4.0f, 5.0f));
    m.AddObstacle(std::make_pair(-4.0f, 6.0f), std::make_pair(4.0f, 6.0f));
    m.AddObstacle(std::make_pair(1.0f, 7.0f), std::make_pair(3.0f, 10.0f));
//    m.AddObstacle(std::make_pair(-1.0f, 7.0f), std::make_pair(-3.0f, 10.0f));
    
    auto path = m.ShortestPathTo(0, 0, 0, 10);
    for(auto i : path)
	std::cout << '(' << i.first << ", " << i.second << ')' << std::endl;
    return(0);
}
