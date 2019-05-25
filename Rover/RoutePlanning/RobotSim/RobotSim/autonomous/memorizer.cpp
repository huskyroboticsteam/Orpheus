#include <algorithm>
#include <cmath>

#include "memorizer.hpp"

// return true if original and challenger are sufficiently different
// so that they should be merged
bool same_obstacle(RP::line original, RP::line challenger)
{
    if (RP::same_point(original.p, challenger.p))
        return RP::same_point(original.q, challenger.q);
    else
        return RP::same_point(original.p, challenger.q) &&
               RP::same_point(original.q, challenger.p);
}

void RP::Memorizer::add_obstacles(const std::vector<line> &new_obstacles)
{
    line merged;
    /*
    iterate over each new obstacle, try to merge it with existing obstacles
    if they can merge (meaning, if they are colinear and overlapping), remove the old 
    obstacles and add the new merged obstacle
    */
    for (const line &newobs : new_obstacles)
    {
        // printf("(%f, %f), (%f, %f)\n", newobs.p.x, newobs.p.y, newobs.q.x, newobs.q.y);
        merged = newobs;
        bool should_add = true;
        int nmerged = 0;
        for (auto it_mobs = obstacles.begin(); it_mobs != obstacles.end();)
        {
            bool can_merge = false;
            // if intersect/overlap
            // merge obstacles, remove vobs and don't add newobs, and add merged obstacles
            line temp = merge(merged, *it_mobs, can_merge);
            if (can_merge)
            {
                // printf("attempted merge: m(%f, %f) (%f, %f) and n(%f, %f) (%f, %f) -> t(%f, %f), (%f, %f)\n",
                //                 it_mobs->p.x, it_mobs->p.y, it_mobs->q.x, it_mobs->q.y,
                //                 newobs.p.x, newobs.p.y, newobs.q.x, newobs.q.y,
                //                 temp.p.x, temp.p.y, temp.q.x, temp.q.y);
                if (!same_obstacle(temp, *it_mobs))
                {
                    merged = temp;
                    it_mobs = obstacles.erase(it_mobs);
                    nmerged++;
                }
                else
                {
                    should_add = false;
                    // printf("Same obstacles t(%f, %f) - (%f, %f) and m(%f, %f) - (%f, %f). Don't merge.\n", temp.p.x, temp.p.y,
                    //     temp.q.x, temp.q.y,
                    //     it_mobs->p.x, it_mobs->p.y, it_mobs->q.x, it_mobs->q.y);
                    break;
                }
            }
            else
            {
                it_mobs++;
            }
        }
        if (should_add)
        {
            obstacles.emplace_back(merged);
        }
    }
}

RP::line RP::Memorizer::merge(const line &o, const line &p, bool &can_merge)
{
    bool colinear = orientation(o.p, o.q, p.p) == 0 &&
                    orientation(o.p, o.q, p.q) == 0;
    if (!colinear)
    {
        can_merge = false;
        // printf("Not colinear\n");
        return o;
    }
    can_merge = true;
    // sort points in an arbitrary but consistent direction
    point points[] = {o.p, o.q, p.p, p.q};
    std::sort(points, points + 4, [](const point &p1, const point &p2) {
        if (fabs(p1.x - p2.x) > 1e-3)
            return fabs(p1.x) > fabs(p2.x);
        else
            return fabs(p1.y) > fabs(p2.y);
    });
    if (fabs(points[3].x - points[0].x) - 1e-2 > fabs(o.q.x - o.p.x) + fabs(p.q.x - p.p.x) ||
        fabs(points[3].y - points[0].y) - 1e-2 > fabs(o.q.y - o.p.y) + fabs(p.q.y - p.p.y))
    {
        can_merge = false;
        // printf("Colinear but not overlapping\n");
        return o;
    }
    // printf("Merging\n");
    // for (auto p : points)
    // {
    //     printf("(%f, %f), ", p.x, p.y);
    // }
    // printf("\n");
    return line{points[0], points[3]};
}
