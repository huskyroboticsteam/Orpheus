#include "RobotEKF.hpp"
#include "Server.hpp"
#include "Tennisball/src/detector.hpp"
#include "autonomous/pather.hpp"
#include "autonomous/timer.hpp"
#include <deque>
#include <iostream>
#include <thread>
namespace RP {
enum TurnState {
    TOWARD_TARGET,
    SURVEY_COUNTERCW,
    SURVEY_CW,
    BACK_TO_TARGET,
    FIND_BALL,
    FINISHED,
};
class Controller {
  public:
    RP::Server server;
    Controller(const point &cur_pos, std::deque<point> targetSites);
    bool setDirection(float heading);
    bool setSpeed(float speed);
    void update();
    void addObstacle(float dist1, float dir1, float dist2, float dir2);
    void foundTennisBall(float dist, float dir);

  private:
    bool sendPacket(signed short speed, unsigned short heading);
    bool sendDestinationPacket();
    float get_target_angle();
    void turn_and_go();
    int state;
    RP::point dst;
    std::deque<point> targetSites;
    std::deque<point> spiralPts;
    float curr_lat;
    float curr_lng;
    float curr_dir;
    bool in_spiral_radius();
    bool found_ball();
    RP::point convertToLatLng(float dist, float dir);
    RobotEKF filter;
    std::thread watchdogThread;
    std::thread receiverThread;
    tb::Detector detector;
    RP::Pather pather;
    bool turning;
    TurnState turnstate;
    float tar_angle; 
    Timer timer;
    float last_move_time;
    float last_move_speed;
    float orig_angle;
};
} // namespace RP
