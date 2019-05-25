#ifndef RP_SIMCONTROLLLER
#define RP_SIMCONTROLLLER

#include "grid.hpp"
#include "agent.hpp"
#include "autonomous/timer.hpp"
#include "autonomous/pather.hpp"

// for how long robot should move until it re-calculates and turns again
// TODO decrease this as robot gets closer to an obstacle
// for how long robot should turn each time
// #define AUTO_TURN_TIME 0.7f

#define AUTO_TURN_RANGE 90

namespace RP
{
enum TurnState
{
  TOWARD_TARGET,
  SURVEY_COUNTERCW,
  SURVEY_CW,
  BACK_TO_TARGET,
  FIND_BALL,
  FINISHED,
};
class SimController
{
public:
  SimController(Grid &grd, Agent &agt, Pather &pth) : grid(grd), agent(agt), pather(pth), speed(1.f){};
  void start_auto();
  // called every some fixed (hopefully) time step
  void tic();
  void stop_auto();
  const bool& just_updated = _just_updated;

private:
  Grid &grid;
  Agent &agent;
  Pather &pather;
  Timer timer;
  bool _just_updated; // whether path was updated in the last call to tic()

  float speed;
  float orig_angle; // angle before turning
  float tar_angle;  // target angle for turning
  bool turning;
  double last_move_time;
  float last_move_speed;
  TurnState turnstate;

  void init_turn();
  void turn_and_go();
  float get_target_angle();
};
} // namespace RP

#endif