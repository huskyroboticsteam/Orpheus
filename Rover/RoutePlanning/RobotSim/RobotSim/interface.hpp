//
//  interface.hpp
//  RobotSim
//
//  Created by Tadeusz Pforte on 4/10/19.
//  Copyright © 2019 Husky Robotics. All rights reserved.
//

#ifndef interface_hpp
#define interface_hpp

#include <stdio.h>

#include "grid.hpp"

class Interface {
public:
    Interface(Grid& inGrid, Agent& inAgent);
    
    // moves the robot forward or backward at a given speed
    // speed should be between 1 and -1, where negative is backwards
    void move(float speed);
    
    // rotates the robot clockwise or counterclockwise at a given speed
    // speed should be between 1 and -1, where negative is counterclockwise
    void turn(float speed);
    
    void turnTo(float goalDirection);
    
    // returns the current position of the robot, in meters, relative to the grid
    // origin x and y increase to the right and to the top respectively
    RP::point currentPosition();
    
    // returns the current rotation of the robot, in degrees increasing
    // counterclockwise value is between 0 and 360, where 0 is true right relative
    // to the grid
    float currentRotation();
    
    // returns all the obstacles currently visible to the robot
    // note that these may be partial obstacles
    // TODO may return an RP::obstacle depending on implementation
    std::vector<RP::line> currentObstaclesInView();
    
private:
    Grid& grid;
    Agent& agent;
};

#endif /* interface_hpp */
