// handles all the pathing logic. has member Mapper that creates graph and possibly members (or methods)
// that handle obstalce merging and pathfinding within graph

// a good idea is to switch between Continuous and Discrete mapping based on the number and density of
// obstacles
#ifndef RP_PATHER_HPP
#define RP_PATHER_HPP

#include "quadMapper.hpp"
#include "memorizer.hpp"

namespace RP
{
class Pather
{
public:
  Pather(point origin, point target, point max_point);
  const point &cur_point = cur;
  const point &tar_point = tar;
  void compute_path();
  const std::vector<line> &mem_obstacles() const;
  point get_cur_next_point();
  const std::vector<point> &get_cur_path() const;
  void add_obstacles(const std::vector<line> &obstacles);
  const graph &d_graph() const;
  void set_pos(const point &pos);
  void set_tar(const point &tar);

  pqtree debug_qtree_root() const { return fineMapper.get_qtree_root(); }

  void reset()
  {
    memorizer.reset();
    fineMapper.reset();
  }

private:
  Memorizer memorizer;
  QuadMapper fineMapper;
  point cur;
  point tar;
  float tol;
  point max_pt;
  std::vector<point> cur_path;

  void prune_path(std::vector<int> &path, float tol);
  float heuristic_cost(const point &p, const point &tar);
};
} // namespace RP

#endif
