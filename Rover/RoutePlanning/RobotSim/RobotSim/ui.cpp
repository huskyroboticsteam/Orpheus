#include "ui.hpp"

sf::VertexArray get_vertex_line(RP::point p, RP::point q, sf::Color c, float scale, float window_height)
{
    sf::VertexArray line;
    line.setPrimitiveType(sf::Lines);
    line.resize(2);
    line[0] = sf::Vertex(sf::Vector2f((p.x + 1) * scale, (window_height - p.y) * scale));
    line[0].color = c;
    line[1] = sf::Vertex(sf::Vector2f((q.x + 1) * scale, (window_height - q.y) * scale));
    line[1].color = c;
    return line;
}
