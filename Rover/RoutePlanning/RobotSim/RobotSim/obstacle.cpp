#include "obstacle.hpp"

Obstacle::Obstacle(float p1x, float p1y, float p2x, float p2y, float mapScale, float mapW, float mapH)
{
    // disabled drawing quadrants because I'm not using quadrants unless I have to later
    drawQuadrants = false;

    scale = mapScale;
    mapWidth = mapW;
    mapHeight = mapH;

    x1 = p1x;
    y1 = p1y;
    x2 = p2x;
    y2 = p2y;

    line.setPrimitiveType(sf::Lines);
    line.resize(2);
    line[0] = sf::Vertex(sf::Vector2f((x1 + 1) * scale, (mapHeight - y1) * scale));
    line[1] = sf::Vertex(sf::Vector2f((x2 + 1) * scale, (mapHeight - y2) * scale));
    line[0].color = color;
    line[1].color = color;
}

// changes the obstacle color to the given new color
void Obstacle::recolor(sf::Color newColor)
{
    line[0].color = newColor;
    line[1].color = newColor;
}

void Obstacle::draw(sf::RenderTarget &target, sf::RenderStates states) const
{
    if (drawQuadrants)
        target.draw(quadrantFill, states);
    target.draw(line, states);
}
