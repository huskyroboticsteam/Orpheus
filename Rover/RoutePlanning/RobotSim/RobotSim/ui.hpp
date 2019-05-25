#ifndef ROVERPATHFINDING_UI_H
#define ROVERPATHFINDING_UI_H

#include <SFML/Graphics.hpp>
#include "autonomous/utils.hpp"

#define THEME 0 // 0 is normal, 1 is HACKERMODE

#define TARGET_COLOR sf::Color:Red;
#if THEME
#define PATH_COLOR sf::Color::Yellow 
#define OBST_COLOR sf::Color(82, 165, 46)
#define VISIBLE_OBST_COLOR sf::Color(178, 255, 145)
#define SEEN_OBST_COLOR sf::Color::Green
#define VIEW_SHAPE_COLOR sf::Color::White
#define GRAPH_EDGE_COLOR sf::Color::Red
#define GRAPH_NODE_COLOR sf::Color::Red
#else
#define PATH_COLOR sf::Color(115, 92, 196);
#define OBST_COLOR sf::Color::Black
#define VISIBLE_OBST_COLOR sf::Color::Green
#define SEEN_OBST_COLOR sf::Color(58, 201, 0)
#define VIEW_SHAPE_COLOR sf::Color::Blue
#define GRAPH_EDGE_COLOR sf::Color::Red
#define GRAPH_NODE_COLOR sf::Color::Red
#endif

sf::VertexArray get_vertex_line(RP::point p, RP::point q, sf::Color c, float scale, float window_height);

#endif