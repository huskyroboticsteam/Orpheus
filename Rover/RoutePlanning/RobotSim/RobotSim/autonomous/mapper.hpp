#ifndef RP_MAPPER_HPP
#define RP_MAPPER_HPP

#include "utils.hpp"
#include <memory>
#include <unordered_map>

namespace RP
{
struct edge
{
  int parent;
  int child;
  float len;
};

struct node
{
  int prev;
  point coord;
  std::unordered_map<int, edge> connection;
  int qt_id; // qtnode id
};

struct graph
{
  std::vector<node> nodes;
  //Adds an edge to the graph; returns parent=>child edge pointer
  void add_edge(int parent, int child); 
  std::unordered_map<int, RP::edge>::iterator remove_edge(int parent, int child);
  //Creates a node. Returns index in nodes of the created node
  int create_node(point coord);
  void clear();
};

class Mapper
{
public:
  Mapper(point origin, point target, float tolerance, const std::vector<line>& all_obstacles);
  const point& cur_point = cur;
  const point& tar_point = tar;
  const graph& d_graph = mygraph;

  virtual void set_pos(point pos) = 0;
  virtual void set_tar(point tar) = 0;
  virtual void set_tol(float tol) = 0;
  virtual void new_obstacles(const std::vector<line>& obstacles) = 0;
  // graph methods used for pather. nodes[0] is always current node, nodes[1] is always target
  virtual void compute_graph() = 0;
  graph get_graph();
  // returns true if the path is unobstructed by obstacles
  virtual bool path_good(int node1, int node2, float tol) const =0;

protected:
  point cur;
  point tar;
  graph mygraph;
  float tol;
  const std::vector<line> &all_obstacles;
};
} // namespace RP

#endif
