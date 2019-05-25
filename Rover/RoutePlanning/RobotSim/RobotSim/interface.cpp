//
//  interface.cpp
//  RobotSim
//
//  Created by Tadeusz Pforte on 4/10/19.
//  Copyright © 2019 Husky Robotics. All rights reserved.
//

#include "interface.hpp"

Interface::Interface(Grid& inGrid, Agent& inAgent) : grid(inGrid), agent(inAgent) {
    // no other construction needed
}

// moves the robot forward or backward at a given speed
// speed should be between 1 and -1, where negative is backwards
void Interface::move(float speed) {
    grid.moveAgent(agent, agent.drive(speed));
    
}

// rotates the robot clockwise or counterclockwise at a given speed
// speed should be between 1 and -1, where negative is counterclockwise
void Interface::turn(float speed) {
    grid.rotateAgent(agent, agent.turn(speed));
}

void Interface::turnTo(float goalDirection) {
    // std::cout << "goal direction: " << goalDirection << std::endl;
    // std::cout << "internal direction: " << agent.getInternalRotation() <<
    // std::endl;
    if (abs(goalDirection - agent.getInternalRotation()) > 1.0)
    {
        if (goalDirection > agent.getInternalRotation())
        {
            turn(-1);
        }
        else
        {
            turn(1);
        }
    }
    else
    {
        turn(0);
    }
}
// returns the current position of the robot, in meters, relative to the grid
// origin x and y increase to the right and to the top respectively
RP::point Interface::currentPosition() {
    return RP::point{agent.getX(), agent.getY()};
}

// returns the current rotation of the robot, in degrees increasing
// counterclockwise value is between 0 and 360, where 0 is true right relative
// to the grid
float Interface::currentRotation() {
    return agent.getInternalRotation();
}

// returns all the obstacles currently visible to the robot
// note that these may be partial obstacles
// TODO may return an RP::obstacle depending on implementation
std::vector<RP::line> Interface::currentObstaclesInView()
{
    // TODO
}
