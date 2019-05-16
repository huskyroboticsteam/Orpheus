#include <vector>
#include <iostream>
#include <fstream>
#include <cctype>
#include <cmath>
#include <assert.h>

#include "Simulator.hpp"
#include "grid.hpp"
#include "ui.hpp"

#define DEBUG_MSG 0

void debugmsg(const char *);

RP::Simulator::Simulator(const std::vector<Obstacle> &obstacleList, const Agent &agt,
                         simulator_config conf, float map_scale, float windowH) : raw_obstacles(obstacleList),
                                                                                  agent(agt), config(conf), scale(map_scale),
                                                                                  window_height(windowH),
                                                                                  vision_dist_sq(std::pow(conf.vision_dist, 2))
{
}

void RP::Simulator::update_agent()
{
    for (auto const &obst : raw_obstacles)
    {
        all_obstacles.push_back(pobst(new sim_obstacle{point{obst.x1, obst.y1}, point{obst.x2, obst.y2}}));
    }
    cur_pos = point{agent.getX(), agent.getY()};
    bearing = agent.getInternalRotation();

    point cur_pos{agent.getX(), agent.getY()};
    upper_vis = normalize_angle(deg_to_rad(bearing + config.vision_angle / 2));
    lower_vis = normalize_angle(deg_to_rad(bearing - config.vision_angle / 2));
    // compute farthest point at lower and upper fov bounds once
    fov_lower = polar_to_cartesian(cur_pos, config.vision_dist, lower_vis);
    fov_upper = polar_to_cartesian(cur_pos, config.vision_dist, upper_vis);
    if (all_obstacles.empty())
    {
        return;
    }

    std::vector<proc_obstacle> cropped_obst;
    // crop obstacles
    for (auto it = all_obstacles.begin(); it != all_obstacles.end();)
    {
        auto aop = *it;
        point p = aop->p;
        point q = aop->q;
        //possible optimization here for large map:
        //remove obstacles if they seem too far (i.e. much farther than vis_dist)
        bool p_within_view = within_view(p);
        bool q_within_view = within_view(q);
        if (p_within_view && q_within_view)
        {
            debugmsg("BOTH endpoints within view");
            proc_obstacle so{p, q, 3};
            aop->index = cropped_obst.size();
            cropped_obst.push_back(so);
        }
        else if (p_within_view || q_within_view)
        {
            debugmsg("ONE endpoints within view");
            point &fixed_pt = p_within_view ? p : q;
            std::vector<point> pts = intersection_with_arc(p, q, fov_lower, fov_upper);
            debugmsg("intersecting with arc");
            // assert(pts.size() == 1);
            if (pts.size() == 1)
            {
                proc_obstacle so{fixed_pt, pts.at(0), 1};
                aop->index = cropped_obst.size();
                cropped_obst.push_back(so);
            }
        }
        else
        {
            debugmsg("NO endpoint within view");
            // neither is within view. check for double intersection with arc
            int sz = all_obstacles.size();
            std::vector<point> pts = intersection_with_arc(p, q, fov_lower, fov_upper);
            if (pts.size() == 2)
            {
                debugmsg("intersecting with arc");
                proc_obstacle so{pts.at(0), pts.at(1), 0};
                aop->index = cropped_obst.size();
                cropped_obst.push_back(so);
            }
            else
            {
                // assert(pts.size() == 0);
                it = all_obstacles.erase(it);
                continue; // don't increment it
                // if (pts.size() != 0)
                // {
                //     intersection_with_arc(p, q, fov_lower, fov_upper);
                // }
            }
        }
        it++;
    }
    // std::cout << cropped_obst.size() << std::endl;
    /*
    Logic:
    for each obstacle, and for each of its endpoints, find the closest intersection between line{cur_pos, endpoint} and 
    any other obstacle. If that intersection is closer to cur_pos than endpoint is, remove this endpoint from obstacle.
    Else, add that intersection point to the obstacle it landed on.
    */
    for (uint i = 0; i < cropped_obst.size(); i++)
    {
        auto &co = cropped_obst.at(i);
        // iterate over two endpoints
        point pts[2] = {co.p, co.q};
        for (uint pi = 0; pi < 2; pi++)
        {
            point p = pts[pi], q = pts[1 - pi];
            // find intersection
            float closest_dist = INFINITY;
            point closest;
            pobst closest_obstacle;
            bool drop_pt = false;
            for (auto aop : all_obstacles)
            {
                if (aop->index == i) // same one
                    continue;

                point s = intersection(cur_pos, p, aop->p, aop->q);
                // test if there is intersection
                if (s.x == INFINITY || !same_dir(cur_pos, p, s) || !within_segment(aop->p, aop->q, s))
                {
                    // if (same_point(p, point{5.f, 5.f}, 1e-7) && same_point(aop->p, point{5.f, 5.f}, 1e-7))
                    // std::cout << "HOO\n";
                    continue;
                }
                if (same_point(s, p, 1e-5))
                {
                    // this is a shared vertex. decide if this should be added
                    // simply see the line intesection of jt and point{cur_pos, q}
                    // where q is the other side point (from p). If the intersect
                    // is in the same direction as q relative to cur_pos, then
                    // this p should be discarded
                    point contending_point;
                    if (same_point(p, aop->p, 1e-5))
                        contending_point = aop->q;
                    else
                        contending_point = aop->p;
                    point inter = intersection(p, contending_point, cur_pos, q);
                    // i.e. there is an intersection, the intersection is not the shared vertex, and
                    // the intersection falls on the contending obstacle. This means that p is a shared
                    // vertex and is blocked
                    if (inter.x != INFINITY && !same_point(inter, p, 1e-5) && same_dir(p, contending_point, inter) && within_segment(cur_pos, q, inter))
                    {
                        drop_pt = true;
                        // if (same_point(p, point{5.f, 10.f}, 1e-7) && same_point(q, point{5.f, 5.f}, 1e-7))
                        // printf("dropping point (%f, %f)\n", p.x, p.y);
                        break;
                    }
                }
                float dist = dist_sq(cur_pos, s);
                if (same_dir(cur_pos, p, s) && dist < closest_dist)
                {
                    closest_dist = dist;
                    closest = s;
                    closest_obstacle = aop;
                }
            }

            if (drop_pt)
                continue;
            float my_dist = dist_sq(cur_pos, p);
            // not blocked
            if (closest_dist + 1e-3 >= my_dist)
            {
                co.endpoints.push_back(p);
                // i.e. p is a side point AND closest point is not the same point as p AND closest_point is in view
                if ((co.sides & (1 << pi)) && closest_dist != INFINITY && closest_dist - my_dist > 1e-3 && closest_dist <= vision_dist_sq)
                {
                    // printf("Adding projection for (%f, %f)\n", closest.x, closest.y);
                    cropped_obst.at(closest_obstacle->index).endpoints.push_back(closest); // add projection
                }
            }
        }
    }
    //note: side rays already accounted for in "crop obstacles"
    //collect and add obstacles
    view_obstacles.clear();
    for (auto &obs : cropped_obst)
    {
        // assert(obs.endpoints.size() % 2 == 0);
        obs.endpoints.sort([](const point &p, const point &q) { return p.x > q.x; });
        for (auto it = obs.endpoints.begin(); it != obs.endpoints.end(); it++)
        {
            point a = *it;
            if (++it == obs.endpoints.end())
                break;
            view_obstacles.push_back(line{a, *it});
        }
    }
    // for (auto & obs : view_obstacles)
    // {
    //     printf("(%f, %f) - (%f, %f); ", obs.p.x, obs.p.y, obs.q.x, obs.q.y);
    // }
    // printf("\n");
    all_obstacles.clear();
}

void debugmsg(const char *line)
{
#if DEBUG_MSG
    std::cout << line << std::endl;
#endif
}

// Find the intersection of a line with the field of view arc (https://stackoverflow.com/a/30012445)
std::vector<RP::point>
RP::Simulator::intersection_with_arc(const point &p1, const point &p2,
                                     const point &lower_point, const point &upper_point)
{
    std::vector<point> ret;
    // check the two extreme rays
    point s1 = segments_intersection(p1, p2, cur_pos, lower_point);
    point s2 = segments_intersection(p1, p2, cur_pos, upper_point);
    if (s1.x != INFINITY)
        ret.push_back(s1);
    if (s2.x != INFINITY)
        ret.push_back(s2);

    if (ret.size() == 2)
        return ret;

    float a = p2.y - p1.y;
    float b = -(p2.x - p1.x);
    float c = p2.x * p1.y - p1.x * p2.y;
    float p = std::atan2(b, a);
    float cosval = -(a * cur_pos.x + b * cur_pos.y + c) / (config.vision_dist * std::sqrt(a * a + b * b));
    if (cosval >= 1 || cosval < -1)
        return ret; // no intersection/tangent
    float q = std::acos(cosval);

    float t1 = normalize_angle(p + q); // first possible angle
    float t2 = normalize_angle(p - q); // second possible angle
    point pt1 = polar_to_cartesian(cur_pos, config.vision_dist, t1);
    point pt2 = polar_to_cartesian(cur_pos, config.vision_dist, t2);
    float within_1 = within_angle(t1, lower_vis, upper_vis);
    float within_2 = within_angle(t2, lower_vis, upper_vis);

    float seg_len = dist_sq(p1, p2);
    if (within_1 && within_segment(p1, p2, pt1))
        ret.push_back(pt1);
    if (within_2 && within_segment(p1, p2, pt2))
        ret.push_back(pt2);
    return ret;
}

bool RP::Simulator::within_view(const point &pt)
{
    // too far?
    if (dist_sq(cur_pos, pt) > vision_dist_sq)
    {
        return false;
    }
    float angle = normalize_angle(std::atan2(pt.y - cur_pos.y, pt.x - cur_pos.x));
    // angle out of view
    if (!within_angle(angle, lower_vis, upper_vis))
    {
        return false;
    }

    // for (auto it = all_obstacles.begin(); it != all_obstacles.end(); it++)
    // {
    //     const point p = it->p;
    //     const point &q = it->q;
    //     if (segments_intersection(p, q, cur_pos, pt).x != INFINITY && (!same_point(p, pt, 1e-4) && !same_point(q, pt, 1e-4)))
    //     { // intersection is not self
    //         return false;
    //     }
    // }
    return true;
}

void RP::Simulator::draw(sf::RenderTarget &target, sf::RenderStates states) const
{
    sf::Color visColor = VIEW_SHAPE_COLOR;
    target.draw(get_vertex_line(cur_pos, fov_lower, visColor, scale, window_height));
    target.draw(get_vertex_line(cur_pos, fov_upper, visColor, scale, window_height));
    std::list<sf::VertexArray> circleLines = getCircleLines(bearing, config.vision_dist, config.vision_angle, cur_pos, 10, VIEW_SHAPE_COLOR);
    for (const auto& seg : circleLines)
        target.draw(seg);
    for (const auto& obst : view_obstacles)
        target.draw(get_vertex_line(obst.p, obst.q, VISIBLE_OBST_COLOR, scale, window_height), states);
}

std::list<sf::VertexArray> RP::Simulator::getCircleLines(float angular_pos, float radius, float angle_spread, RP::point pos, int maxpts, sf::Color clr) const
{
    std::vector<RP::point> points;
    const float lower = deg_to_rad(angular_pos - angle_spread / 2.f);
    const float inc = deg_to_rad(angle_spread / (maxpts - 1));
    for (int i = 0; i < maxpts; ++i)
    {
        const float a = lower + i * inc;
        points.push_back(RP::point{pos.x + radius * std::cos(a), pos.y + radius * std::sin(a)});
    }
    std::list<sf::VertexArray> ret;
    for (uint i = 0; i < maxpts - 1; i++)
    {
        ret.push_back(get_vertex_line(points.at(i), points.at(i + 1), clr, scale, window_height));
    }
    return ret;
}

RP::point RP::Simulator::getpos()
{
    return cur_pos;
}
