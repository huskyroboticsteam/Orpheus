//
//  grid.cpp
//  RobotSim
//
//  Created by Tadeusz Pforte on 12/4/18.
//  Copyright © 2018 Husky Robotics. All rights reserved.
//

#include <iostream>
#include <fstream>
#include <sstream>
#include <math.h>

#include "grid.hpp"
#include "autonomous/utils.hpp"
#include "ui.hpp"

// creates a grid that can have obstacles and agents that it moves around
// !!! NOTE !!!
// For aesthetic purposes and to be a pain in the ass,
// the grid is not drawn starting at (0, 0) in the window.
// Instead it starts at (1 meter, 1 meter), aka (scale, scale) in the window.
// Things you draw, if they are using the grid's scale, will need to be drawn
// with this 1 meter offset taken into account.
Grid::Grid(float w, float h, unsigned int s)
{
    width = w;
    height = h;
    scale = s;
    target = RP::point{-1, -1};

    // TOP_BORDER = {0.f, 0.f, width - 1, 0.f};
    // RIGHT_BORDER = {width - 1, 0.f, width - 1, height - 1};
    // BOTTOM_BORDER = {0.f, height - 1, width - 1, height - 1};
    // LEFT_BORDER = {0.f, 0.f, 0.f, height - 1};

    showGrid = false;
    noclip = false;

    // border.setPrimitiveType(sf::LinesStrip);
    // border.resize(5);

    // border[0] = sf::Vector2f(scale, scale);
    // border[1] = sf::Vector2f(scale, scale * width);
    // border[2] = sf::Vector2f(scale * height, scale * width);
    // border[3] = sf::Vector2f(scale * height, scale);
    // border[4] = sf::Vector2f(scale, scale);

    // border[0].color = BORDER_COLOR;
    // border[1].color = BORDER_COLOR;
    // border[2].color = BORDER_COLOR;
    // border[3].color = BORDER_COLOR;
    // border[4].color = BORDER_COLOR;

    gridlines.setPrimitiveType(sf::Lines);
    for (int x = 1; x < width; x++)
    {
        for (int y = 1; y < height; y++)
        {
            gridlines.append(sf::Vector2f(scale * x, scale * y));
            gridlines.append(sf::Vector2f(scale * x, scale * (y + 1)));
            gridlines.append(sf::Vector2f(scale * x, scale * y));
            gridlines.append(sf::Vector2f(scale * (x + 1), scale * y));
            gridlines[gridlines.getVertexCount() - 4].color = GRID_COLOR;
            gridlines[gridlines.getVertexCount() - 3].color = GRID_COLOR;
            gridlines[gridlines.getVertexCount() - 2].color = GRID_COLOR;
            gridlines[gridlines.getVertexCount() - 1].color = GRID_COLOR;
        }
    }

    currentPath.setPrimitiveType(sf::Lines);
}

// toggles whether or not gridlines are drawn every meter
void Grid::toggleGrid()
{
    showGrid = !showGrid;
}

// toggles whether or not the agent collides with obstacles and borders
void Grid::toggleClipping()
{
    printf("toggling noclip\n");
    if (noclip)
        debugMsg("Clipping toggled on");
    else
        debugMsg("Clipping toggled off");

    noclip = !noclip;
}

bool Grid::drawPath()
{
    currentPath.clear();
    return true;
}

bool Grid::drawPath(std::vector<RP::point> path, Agent &agent)
{
    if (path.size() < 1)
        return false;

    currentPath.clear();
    path.insert(path.begin(), RP::point{agent.getX(), agent.getY()});

    for (unsigned int i = 0; i < path.size() - 1; i++)
    {

        RP::line line{path.at(i), path.at(i + 1)};

        // float x1 = path.at(i).x + 1;
        // float y1 = height - path.at(i).y;
        // float x2 = path.at(i + 1).x + 1;
        // float y2 = height - path.at(i + 1).y;

        // std::cout << x1 << "," << y1 << std::endl;
        // std::cout << x2 << "," << y2 << std::endl;

        // x1 *= scale;
        // y1 *= scale;
        // x2 *= scale;
        // y2 *= scale;

        line.p.x = (line.p.x + 1) * scale;
        line.q.x = (line.q.x + 1) * scale;
        line.p.y = (height - line.p.y) * scale;
        line.q.y = (height - line.q.y) * scale;

        sf::Vertex start(sf::Vector2f(line.p.x, line.p.y));
        sf::Vertex end(sf::Vector2f(line.q.x, line.q.y));

        start.color = sf::Color::Blue;
        end.color = sf::Color::Blue;

        currentPath.append(start);
        currentPath.append(end);
    }

    return true;
}

// reads obstacles from a file
// expects four floats per line, corresponding to the (x, y) of the start and end points in that order
// blank lines are ignored
// any line starting with a non-number (ie. text) is considered a comment and ignored
void Grid::readObstaclesFromFile(std::string fileName)
{
    std::ifstream file;
    file.open(fileName);
    if (file)
    {
        std::string line;

        float x1, y1, x2, y2;
        while (getline(file, line))
        {
            std::istringstream in(line);
            // text is ignored as a comment
            if (!(in >> x1))
                continue;

            in >> y1 >> x2 >> y2;

            placeObstacle(x1, y1, x2, y2);
        }
        if (obstacleList.empty())
            debugMsg("Warning: no obstacle loaded");
    }
    file.close();
}

void Grid::addBorderObstacles()
{
    placeObstacle(0.f, 0.f, width - 1, 0.f);
    // placeObstacle(width - 1, 0.f, width - 1, height - 1);
    // placeObstacle(0.f, height - 1, width - 1, height - 1);
    // placeObstacle(0.f, 0.f, 0.f, height - 1);
}

// creates a new obstacle from (x1, y1) to (x2, y2)
void Grid::placeObstacle(float x1, float y1, float x2, float y2)
{
    obstacleList.push_back(Obstacle(x1, y1, x2, y2, scale, width, height));
}

// moves the agent forward by distance ds (which can be negative)
// if clipping is enabled, collisions with obstacles and borders will block movement
sf::Vertex Grid::moveAgent(Agent &agent, float ds)
{
    // convert internal rotation (counter-clockwise) to SFML rotation (clockwise)
    float curR = -agent.getInternalRotation();

    float xOffset = ds * cos(curR * PI / 180);
    // convert y from SFML (top is 0) to internal position (bottom is 0)
    float yOffset = -ds * sin(curR * PI / 180);

    if (!willCollide(agent, xOffset, yOffset, 0))
    {
        agent.move(xOffset, yOffset);
    }
    return agent.getPosition();
}

// rotates the agent clockwise by the angle dr (which can be negative)
// if clipping is enabled, collisions with obstacles and borders will block rotation
float Grid::rotateAgent(Agent &agent, float dr)
{
    if (!willCollide(agent, 0, 0, dr))
        agent.rotate(dr);
    return 0.f;
}

// returns true if the two lines, stored as {x1, y1, x2, y2}, intersect
// otherwise returns false
// sorry for variable names, but believe me it works 100%
bool Grid::linesCollide(RP::line line1, RP::line line2)
{
    float l1x1 = line1.p.x;
    float l1y1 = line1.p.y;
    float l1x2 = line1.q.x;
    float l1y2 = line1.q.y;

    float l2x1 = line2.p.x;
    float l2y1 = line2.p.y;
    float l2x2 = line2.q.x;
    float l2y2 = line2.q.y;

    float uA = ((l2x2 - l2x1) * (l1y1 - l2y1) - (l2y2 - l2y1) * (l1x1 - l2x1)) / ((l2y2 - l2y1) * (l1x2 - l1x1) - (l2x2 - l2x1) * (l1y2 - l1y1));

    float uB = ((l1x2 - l1x1) * (l1y1 - l2y1) - (l1y2 - l1y1) * (l1x1 - l2x1)) / ((l2y2 - l2y1) * (l1x2 - l1x1) - (l2x2 - l2x1) * (l1y2 - l1y1));

    return 0 <= uA && uA <= 1 && 0 <= uB && uB <= 1;
}

// returns true if any of four lines in an array collides with the other given line
// otherwise returns false
// expects all lines to be arrays storing {x1, y1, x2, y2} in that order
bool Grid::boxCollision(std::array<RP::line, 4> box, RP::line line)
{
    bool flag = false;
    for (RP::line boxLine : box)
    {
        if (linesCollide(boxLine, line))
        {
            flag = true;
            break;
        }
    }
    return flag;
}

// returns true if the agent will collide with nothing in the transformation (dx, dy, dr)
// returns false if there will be no collisions
// is not very sophisticated (simply checks target location instead of path), TODO make better
// currently only checks the map borders
bool Grid::willCollide(Agent &agent, float dx, float dy, float dr)
{
    if (noclip)
        return false;

    // int xQuadrant = agent.getX() / 4;
    // int yQuadrant = agent.getY() / 4;

    std::array<RP::polarPoint, 4> hitbox = agent.getHitBox();
    std::array<RP::line, 4> hitboxLines;

    // apply rotation to hitbox
    for (int i = 0; i < 4; i++)
    {
        hitbox[i].th += (agent.getInternalRotation() + dr + 90) * PI / 180.f;
    }

    // convert to cartesian, and then create the lines
    for (int i = 0; i < 4; i++)
    {
        int end = i + 1;
        if (end > 3)
            end = 0;

        float r1 = hitbox[i].r;
        float t1 = hitbox[i].th;
        float r2 = hitbox[end].r;
        float t2 = hitbox[end].th;

        hitboxLines[i] = RP::line(r1 * cos(t1), r1 * sin(t1), r2 * cos(t2), r2 * sin(t2));

        hitboxLines[i].p.x += agent.getX() + dx;
        hitboxLines[i].p.y += agent.getY() + dy;
        hitboxLines[i].q.x += agent.getX() + dx;
        hitboxLines[i].q.y += agent.getY() + dy;
    }

    bool flag = false;

    // check edge collisions
    // if (xQuadrant == 0 && boxCollision(hitboxLines, LEFT_BORDER)) {
    //     flag = true;
    //     debugMsg("Hit left border");
    // }
    // else if (yQuadrant == 0 && boxCollision(hitboxLines, TOP_BORDER)) {
    //     flag = true;
    //     debugMsg("Hit top border");
    // }
    // else if (xQuadrant == (width / 4 - 1) && boxCollision(hitboxLines, RIGHT_BORDER)) {
    //     flag = true;
    //     debugMsg("Hit right border");
    // }
    // else if (yQuadrant == (height / 4 - 1) && boxCollision(hitboxLines, BOTTOM_BORDER)) {
    //     flag = true;
    //     debugMsg("Hit bottom border");
    // }

    for (Obstacle o : obstacleList)
    {
        //if (boxCollision(hitboxLines, {o.x1, o.y1, o.x2, o.y2}))
        if (boxCollision(hitboxLines, RP::line(o.x1, o.y1, o.x2, o.y2)))
        {
            flag = true;
            debugMsg("Hit obstacle");
            break;
        }
    }

    return flag;
}

void Grid::draw(sf::RenderTarget &renderTarget, sf::RenderStates states) const
{
    states.transform *= getTransform();

    if (showGrid)
        renderTarget.draw(gridlines, states);
    for (Obstacle o : obstacleList)
    {
        renderTarget.draw(o, states);
        // target.draw(border, states);
    }

    renderTarget.draw(currentPath, states);

#define TARGET_SZ 1.f
    sf::Color targetColor = THEME ? sf::Color::Red : sf::Color::Magenta;
    renderTarget.draw(get_vertex_line(RP::point{target.x - TARGET_SZ, target.y - TARGET_SZ}, RP::point{target.x + TARGET_SZ, target.y + TARGET_SZ},
                                      targetColor, scale, height),
                      states);
    renderTarget.draw(get_vertex_line(RP::point{target.x - TARGET_SZ, target.y + TARGET_SZ}, RP::point{target.x + TARGET_SZ, target.y - TARGET_SZ},
                                      targetColor, scale, height),
                      states);
#undef TARGET_SZ
}

void Grid::debugMsg(std::string msg)
{
    // std::cout << msg << std::endl;
}
