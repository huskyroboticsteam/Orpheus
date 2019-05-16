#include "pather.hpp"
#include <queue>
#include <numeric>
#include <algorithm>
#include <cmath>
#include <unordered_set>
#include <cassert>
#include "timer.hpp"

// change level here (this is a landmark for search)
RP::Pather::Pather(point origin, point target, point max_point) : fineMapper(origin, target, memorizer.obstacles_ref, max_point.x, max_point.y, 6),
                                                                  cur(origin), tar(target), max_pt(max_point)
{
}

float RP::Pather::heuristic_cost(const point &p, const point &tar)
{
    return dist_sq(p, tar);
}

//TODO(sasha): Find heuristics and upgrade to A*
// *low priority
void RP::Pather::compute_path()
{
    float tolerances[]{1.f, 0.5f, 0.25f, 0.125f};
    // size_t tol_len = arrlen(tolerances);
    for (int tol_ind = 0; tol_ind < 4; tol_ind++)
    {
        float tol = tolerances[tol_ind];
        // printf("%f\n", tol);
        fineMapper.set_tol(tol);
        Timer comptim;
        fineMapper.compute_graph();
        printf("compute graph took %f seconds\n", comptim.elapsed());
        graph g = fineMapper.get_graph();

#if 0
    for(int i = 0; i < nodes.size(); i++)
    {
	std::cout << "Node " << i << " at (" << nodes[i].coord.first << ", " << nodes[i].coord.second << ") connected to: " << std::endl << '\t';
	for(auto con : nodes[i].connection)
	    std::cout << con.first << ' ';
	std::cout << std::endl;
    }
#endif
        Timer tim;
        // A star source https://en.wikipedia.org/wiki/A*_search_algorithm
        std::unordered_map<int, float> fscore;
        std::unordered_map<int, float> gscore;
        // auto cmp = [fscore](int l, int r) {
        //     auto lit = fscore.find(l);
        //     auto rit = fscore.find(r);
        //     float lfscore = lit == fscore.end() ? INFINITY : lit->second;
        //     float rfscore = rit == fscore.end() ? INFINITY : rit->second;
        //     return lfscore < rfscore;
        // };
        auto cmp = [&gscore](int l, int r) {
            assert(gscore.find(l) != gscore.end());
            assert(gscore.find(r) != gscore.end());
            return gscore[l] > gscore[r];
        };
        std::priority_queue<int, std::vector<int>, decltype(cmp)> q(cmp);
        // fscore[0] = heuristic_cost(g.nodes[0].coord, g.nodes[1].coord);
        // std::unordered_set<int> closed;
        // std::unordered_set<int> open;
        // q.push(0);
        // open.insert(0);
        gscore[0] = 0;
        q.push(0);
        for (int i = 1; i < g.nodes.size(); i++)
        {
            gscore.emplace(i, INFINITY);
            // q.push(i);
        }
        int count = 0;
        while (!q.empty())
        {
            count++;
            int n = q.top();
            // if (n == 1)
            //     break;
            q.pop();
            assert(gscore[n] != INFINITY);

            // open.erase(open.find(n));
            // closed.insert(n);

            for (const auto &pair : g.nodes[n].connection)
            {
                // if (closed.find(pair.first) != closed.end())
                //     continue;
                // assert(gscore.find(n) != gscore.end());
                float dist = gscore[n] + pair.second.len;

                // if (open.find(pair.first) == open.end())
                // {
                    // open.insert(pair.first);
                // } else 
                if (dist >= gscore[pair.first])
                {
                    continue;
                }

                // first is the same as second.child
                g.nodes[pair.first].prev = n;
                gscore[pair.first] = dist;
                q.push(pair.first);
                // fscore[pair.first] = gscore[pair.first] +
                //                      heuristic_cost(g.nodes[pair.first].coord, g.nodes[1].coord);
            }
        }
        // assert(q.size() == open.size());
        // printf("visited %d nodes\n", count);
        std::vector<int> pathIndices;
        int i = 1;
        bool pathFound = true;
        while (i != 0)
        {
            if (i == -1)
            {
                pathFound = false;
                break;
            }
            node &n = g.nodes[i];
            // printf("node: %f, %f\n", n.coord.x, n.coord.y);
            pathIndices.push_back(i);
            i = n.prev;
        }
        // path not found. resort to node closest to target
        // if (!pathFound)
        // {
        //     printf("WARNING: target not found\n");
        //     const node &tar = g.nodes[1];
        //     std::vector<size_t> indices(g.nodes.size());
        //     iota(indices.begin(), indices.end(), 0);
        //     // sort by closest to tar
        //     std::sort(indices.begin() + 2, indices.end(),
        //               [g, tar](size_t i, size_t j) {
        //                   return dist_sq(g.nodes[i].coord, tar.coord) < dist_sq(g.nodes[j].coord, tar.coord);
        //               });

        //     // assertGraph(nodes);
        //     int i = -1;
        //     for (auto it = indices.begin() + 2; it != indices.end(); it++)
        //     {
        //         // if (*it == 3)
        //         //     printf("biatch");
        //         pathIndices.clear();
        //         i = *it;
        //         while (i != 0)
        //         {
        //             if (i < 0)
        //                 break;
        //             node &n = g.nodes[i];
        //             // printf("node: %f, %f\n", n.coord.x, n.coord.y);
        //             pathIndices.push_back(i);
        //             i = n.prev;
        //         }
        //         if (i >= 0)
        //             break;
        //     }

        if (i < 0)
        {
          continue;
        }
        // }
        if (!pathFound)
            printf("ERR: path not found\n");
        printf("pathfinding took %f seconds\n", tim.elapsed());
        std::reverse(pathIndices.begin(), pathIndices.end());
        prune_path(pathIndices, tol + 1.f);
        std::vector<point> result;

        for (int ind : pathIndices)
            result.push_back(g.nodes[ind].coord);
        cur_path = (result);
        return;
    }
    printf("WARNING: completely trapped.\n");
    cur_path = std::vector<point>();
}

RP::point RP::Pather::get_cur_next_point()
{
    if (cur_path.size() == 0)
        return point{INFINITY, INFINITY};
    return cur_path.front();
}

// remove unnecessary nodes from the path (i.e. if removing it does
// not introduce intersections) to increase stability and speed
void RP::Pather::prune_path(std::vector<int> &path, float tol)
{
    unsigned int i = 1;
    path.insert(path.begin(), 0);

    while (i < path.size() - 1)
    {
        if (fineMapper.path_good(path[i - 1], path[i + 1], tol))
        {
            path.erase(path.begin() + i);
        }
        else
        {
            i++;
        }
    }
    path.erase(path.begin());
}

void RP::Pather::add_obstacles(const std::vector<line> &obstacles)
{
    memorizer.add_obstacles(obstacles);
    fineMapper.new_obstacles(obstacles);
}

const RP::graph &RP::Pather::d_graph() const
{
    return fineMapper.d_graph;
}

const std::vector<RP::line> &RP::Pather::mem_obstacles() const
{
    return memorizer.obstacles_ref;
}

const std::vector<RP::point> &RP::Pather::get_cur_path() const
{
    return cur_path;
}

void RP::Pather::set_pos(const point &p)
{
    cur = p;
    fineMapper.set_pos(p);
}

void RP::Pather::set_tar(const point &t)
{
    tar = t;
    fineMapper.set_tar(t);
}
