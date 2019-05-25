#ifndef ROVERPATHFINDING_SIMULATOR_H
#define ROVERPATHFINDING_SIMULATOR_H

#include <string>
#include <memory>
#include <list>
#include "obstacle.hpp"
#include "agent.hpp"
#include "autonomous/mapper.hpp"

#define MAX_GRID_RESOLUTION 100 // max number of grid cells on each side of the map

namespace RP
{
struct simulator_config
{
  float vision_angle; //Angle of FOV in degrees
  float vision_dist;  //Distance of vision in meters
};

struct sim_obstacle
{
  point p;
  point q;
  uint index; // index of corresponding "parent" obstacle
};

struct proc_obstacle
{
  point p;
  point q;
  char sides; // 0 if neither is side, 1 if p, 2 if q, 3 if both
  std::list<RP::point> endpoints;
};
typedef std::shared_ptr<sim_obstacle> pobst;

class Simulator : public sf::Drawable
{
public:
  Simulator(const std::vector<Obstacle> &obstacleList, const Agent &agent, simulator_config conf, float map_scale, float windowH);
  const std::vector<line> &visible_obstacles() { return view_obstacles; };
  void update_agent();
  point getpos();

private:
  std::vector<point> intersection_with_arc(const point &p, const point &q, const point &lower_point, const point &upper_point);
  bool within_view(const point &pt);
  float scale;         // scale is only used when calling draw
  float window_height; // only used when calling draw
  const Agent &agent;

  // for computations; not actual attributes of the system
  point cur_pos;   //updated every cal to update_agent()
  float bearing;
  float lower_vis; //Bearing subtracted by half of vision_angle (lower bound of FoV) for caching
  float upper_vis; //Bearing added by half of vision_angle (upper bound of FoV)
  float vision_dist_sq;
  point fov_lower;
  point fov_upper;

  point target_pos;
  simulator_config config;
  const std::vector<Obstacle> &raw_obstacles;
  std::vector<pobst> all_obstacles;
  std::vector<line> view_obstacles;

  virtual void draw(sf::RenderTarget &target, sf::RenderStates states) const;
  std::list<sf::VertexArray> getCircleLines(float angular_pos, float radius, float angle_spread, point pos, int maxpts, sf::Color clr) const;
};
} // namespace RP

#endif
