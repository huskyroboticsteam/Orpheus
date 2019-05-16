//
//  grid.hpp
//  RobotSim
//
//  Created by Tadeusz Pforte on 1/8/19.
//  Copyright © 2019 Husky Robotics. All rights reserved.
//
#ifndef ROBOTSIM_GRID_H
#define ROBOTSIM_GRID_H

#include <stdio.h>
#include <SFML/Graphics.hpp>
#include "autonomous/utils.hpp"
#include "agent.hpp"
#include "obstacle.hpp"

class Grid : public sf::Drawable, public sf::Transformable
{
public:
  Grid(float w, float h, unsigned int s);

  void toggleGrid();
  void toggleClipping();

  void readObstaclesFromFile(std::string filename);
  void addBorderObstacles();
  void placeObstacle(float x1, float y1, float x2, float y2);

  sf::Vertex moveAgent(Agent &agent, float ds);
  float rotateAgent(Agent &agent, float dr);

  unsigned int retrieveScale() { return scale; }
  float retrieveWidth() { return width; }
  float retrieveHeight() { return height; }

  bool linesCollide(RP::line line1, RP::line line2);
  bool boxCollision(std::array<RP::line, 4> box, RP::line line);
  bool willCollide(Agent &agent, float dx, float dy, float dr);

  bool drawPath();
  bool drawPath(std::vector<RP::point> path, Agent &agent);

#if THEME == 0
  sf::Color BORDER_COLOR = sf::Color::Black;
  sf::Color GRID_COLOR = sf::Color(128, 128, 128);
#else
  sf::Color BORDER_COLOR = sf::Color(0, 255, 0);
  sf::Color GRID_COLOR = sf::Color(0, 255, 0);
#endif

  std::vector<Obstacle> obstacleList;
  RP::point target;

private:
  virtual void draw(sf::RenderTarget &target, sf::RenderStates states) const;

  sf::VertexArray border;
  sf::VertexArray gridlines;

  sf::VertexArray currentPath;

  bool showGrid;
  bool noclip;

  RP::line TOP_BORDER;
  RP::line RIGHT_BORDER;
  RP::line BOTTOM_BORDER;
  RP::line LEFT_BORDER;

  float width;        // in meters
  float height;       // in meters
  unsigned int scale; // pixels per meter

  void debugMsg(std::string msg);

};

#endif
