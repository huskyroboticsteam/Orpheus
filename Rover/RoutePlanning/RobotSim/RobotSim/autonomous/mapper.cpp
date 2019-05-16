#include "mapper.hpp"
#include <cassert>
#include <cmath>

RP::Mapper::Mapper(point origin, point target, float tolerance, const std::vector<line>& allobst) : 
cur(origin), tar(target), tol(tolerance), all_obstacles(allobst)
{

}

void RP::graph::add_edge(int parent, int child)
{
    assert(parent >= 0 && child >= 0);
    float dist = sqrt(dist_sq(nodes[parent].coord, nodes[child].coord));

    if (nodes[parent].connection.find(child) != nodes[parent].connection.end())
        return;
    nodes[parent].connection.emplace(child, edge{parent, child, dist});

    nodes[child].connection.emplace(parent, edge{child, parent, dist});
}

std::unordered_map<int, RP::edge>::iterator RP::graph::remove_edge(int parent, int child)
{
    assert(parent >= 0 && child >= 0);
    auto &conn = nodes.at(parent).connection;
    auto it = conn.find(child);
    if (it == conn.end()) {
        printf("WARNING: edge not removed. Parent: %d, child %d\n", parent, child);
        
    }
    auto ret = conn.erase(it);

    auto &conn2 = nodes.at(child).connection;
    it = conn2.find(parent);
    if (it == conn2.end())
        printf("WARNING: edge not removed (reverse). Parent: %d, child %d\n", parent, child);
    conn2.erase(it);
    return ret;
}

int RP::graph::create_node(point coord)
{
    node n;
    n.prev = -1;
    n.coord = coord;
    n.qt_id = -1;
    nodes.push_back(n);
    return (nodes.size() - 1);
}

void RP::graph::clear()
{
    nodes.clear();
}

RP::graph RP::Mapper::get_graph() {
    return mygraph;
}
