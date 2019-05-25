#include "pather.hpp"
#include <queue>
#include <numeric>
#include <algorithm>
#include <cmath>

RP::Pather::Pather(point origin, point target, point max_point) :
           fineMapper(origin, target, memorizer.obstacles_ref, max_point.x, max_point.y, 6),
           cur(origin), tar(target), max_pt(max_point)
{
}

//TODO(sasha): Find heuristics and upgrade to A*
// *low priority
void RP::Pather::compute_path()
{
    float tolerances[]{0.5f, 0.25f, 0.125f, 0.f};
    // size_t tol_len = arrlen(tolerances);
    for (int tol_ind = 0; tol_ind < 4; tol_ind++)
    {
        float tol = tolerances[tol_ind];
        // printf("%f\n", tol);
        fineMapper.set_tol(tol);
        fineMapper.compute_graph();
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
        auto cmp = [g](int l, int r) { return g.nodes[l].dist_to < g.nodes[r].dist_to; };
        std::priority_queue<int, std::vector<int>, decltype(cmp)> q(cmp);
        q.push(0);
        while (!q.empty())
        {
            int n = q.top();
            q.pop();

            for (const auto& pair : g.nodes[n].connection)
            {
                float dist = g.nodes[n].dist_to + pair.second.len;
                if (dist < g.nodes[pair.second.child].dist_to)
                {
                    g.nodes[pair.second.child].prev = n;
                    g.nodes[pair.second.child].dist_to = dist;
                    q.push(pair.second.child);
                }
            }
        }
        std::vector<int> pathIndices;
        int i = 1;
        bool pathFound = true;
        while (i != 0)
        {
            if (i == -1)
            {
                // printf("No path to target found. Resorting to node closest to target\n");
                pathFound = false;
                break;
            }
            node &n = g.nodes[i];
            // printf("node: %f, %f\n", n.coord.x, n.coord.y);
            pathIndices.push_back(i);
            i = n.prev;
        }
        // path not found. resort to node closest to target
        if (!pathFound)
        {
            const node &tar = g.nodes[1];
            std::vector<size_t> indices(g.nodes.size());
            iota(indices.begin(), indices.end(), 0);
            // sort by closest to tar
            std::sort(indices.begin() + 2, indices.end(),
                      [g, tar](size_t i, size_t j) {
                          return dist_sq(g.nodes[i].coord, tar.coord) < dist_sq(g.nodes[j].coord, tar.coord);
                      });

            // assertGraph(nodes);
            int i = -1;
            for (auto it = indices.begin() + 2; it != indices.end(); it++)
            {
                // if (*it == 3)
                //     printf("biatch");
                pathIndices.clear();
                i = *it;
                while (i != 0)
                {
                    if (i < 0)
                        break;
                    node &n = g.nodes[i];
                    // printf("node: %f, %f\n", n.coord.x, n.coord.y);
                    pathIndices.push_back(i);
                    i = n.prev;
                }
                if (i >= 0)
                    break;
            }

            if (i < 0)
            {
                continue;
            }
        }
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

/* TODO: re-implement this */
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
