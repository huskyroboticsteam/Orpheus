#include "quadMapper.hpp"
#include <queue>
#include <cassert>
#include <cmath>

// get code from https://geidav.wordpress.com/2017/12/02/advanced-octrees-4-finding-neighbor-nodes/
// kinda know how it works but not didn't exactly go into it -gary
RP::pqtree RP::QTreeNode::get_neighbor_ge(Direction dir)
{
    pqtree nd;
    switch (dir)
    {
    case UP:
        if (!parent)
            return pqtree(nullptr);

        if (parent->botleft.get() == this)
            return parent->topleft;

        if (parent->botright.get() == this)
            return parent->topright;

        nd = parent->get_neighbor_ge(dir);
        if (!nd || nd->is_leaf)
            return nd;

        // this must be a top child
        return parent->topleft.get() == this ? nd->botleft : nd->botright;
    case DOWN:
        if (!parent)
            return pqtree(nullptr);

        if (parent->topleft.get() == this)
            return parent->botleft;

        if (parent->topright.get() == this)
            return parent->botright;

        nd = parent->get_neighbor_ge(dir);
        if (!nd || nd->is_leaf)
            return nd;

        return parent->botleft.get() == this ? nd->topleft : nd->topright;
    case LEFT:
        if (!parent)
            return pqtree(nullptr);

        if (parent->topright.get() == this)
            return parent->topleft;

        if (parent->botright.get() == this)
            return parent->botleft;

        nd = parent->get_neighbor_ge(dir);
        if (!nd || nd->is_leaf)
            return nd;

        return parent->topleft.get() == this ? nd->topright : nd->botright;
    case RIGHT:
        if (!parent)
            return pqtree(nullptr);

        if (parent->topleft.get() == this)
            return parent->topright;

        if (parent->botleft.get() == this)
            return parent->botright;

        nd = parent->get_neighbor_ge(dir);
        if (!nd || nd->is_leaf)
            return nd;

        return parent->topright.get() == this ? nd->topleft : nd->botleft;
    default:
        printf("WARNING: unrecognized direction argument in QTreeNode::get_neighbor_ge()\n");
        return nullptr;
    }
}

RP::pqtree RP::QuadMapper::create_qtnode(float minx, float miny, float maxx, float maxy, int lvl)
{
    pqtree created = pqtree(new QTreeNode(minx, miny, maxx, maxy, lvl, qtnodes.size()));
    qtnodes.push_back(created);
    return created;
}

RP::QuadMapper::QuadMapper(const point &cur_pos, const point &target, const std::vector<line> &allobst,
                           float fwidth, float fheight, int max_d, float tolerance) : Mapper(cur_pos, target, tolerance, allobst),
                                                                                      max_depth(max_d), field_width(fwidth), field_height(fheight),
                                                                                      cur_changed(true), tar_changed(true)
{
    reset();
}

void RP::QuadMapper::set_pos(point c)
{
    cur = c;
    mygraph.nodes[0].coord = c;
    cur_changed = true;
}

void RP::QuadMapper::set_tar(point t)
{
    tar = t;
    mygraph.nodes[1].coord = t;
    tar_changed = true;
}

void RP::QuadMapper::set_tol(float t)
{
    if (t != tol)
    {
        tol = t;
        reset();
        new_obstacles(all_obstacles);
    }
}

void RP::QuadMapper::init_graph()
{
    mygraph.clear();
    mygraph.create_node(cur);
    mygraph.create_node(tar);
}

void RP::QuadMapper::new_obstacles(const std::vector<line> &obstacles)
{
    std::queue<pqtree> q;
    for (const line &o : obstacles)
    {
        line obs = add_length_to_line_segment(o.p, o.q, tol);
        line left = get_moved_line(obs, tol, false);
        line right = get_moved_line(obs, tol, true);
        line sides []{left, line{left.q, right.q}, line{right.q, right.p}, line{right.p, left.p}};
        q.push(root);
        while (!q.empty())
        {
            pqtree nd = q.front();
            q.pop();

            if (rect_intersects_rect(sides, nd->sides))
            {
                // if depth limit reahed, don't split anymore
                if (nd->depth >= max_depth)
                {
                    nd->is_blocked = true;
                    const auto& conn = mygraph.nodes[nd->graph_id].connection;
                    if (nd->graph_id != -1)
                        for (auto it = conn.begin(); it != conn.end(); it = mygraph.remove_edge(it->second.parent, it->second.child)) {
                            // Gary, this is terrible
                        }
                }
                else
                {
                    if (nd->is_leaf)
                    {
                        nd->is_leaf = false;
                        // split node
                        float mid_x = (nd->min_x + nd->max_x) / 2;
                        float mid_y = (nd->min_y + nd->max_y) / 2;
                        int next_depth = nd->depth + 1;
                        pqtree bl = create_qtnode(nd->min_x, nd->min_y, mid_x, mid_y, next_depth);
                        nd->botleft = bl;
                        nd->botleft->parent = nd;
                        nd->botright = create_qtnode(mid_x, nd->min_y,
                                                     nd->max_x, mid_y, next_depth);
                        nd->botright->parent = nd;
                        nd->topleft = create_qtnode(nd->min_x, mid_y,
                                                    mid_x, nd->max_y, next_depth);
                        nd->topleft->parent = nd;
                        nd->topright = create_qtnode(mid_x, mid_y, nd->max_x,
                                                     nd->max_y, next_depth);
                        nd->topright->parent = nd;

                        // already in graph
                        if (nd->graph_id != -1)
                        {
                            if (nd->qt_id == 10)
                            {
                                printf("boom\n");
                            }
                            removed_nodes.insert(nd->qt_id);
                        }
                        else
                        {
                            auto it = new_nodes.find(nd->qt_id);
                            assert(it != new_nodes.end());
                            new_nodes.erase(it);
                        }

                        new_nodes.insert(nd->topleft->qt_id);
                        new_nodes.insert(nd->topright->qt_id);
                        new_nodes.insert(nd->botleft->qt_id);
                        new_nodes.insert(nd->botright->qt_id);
                    }
                    q.push(nd->botleft);
                    q.push(nd->botright);
                    q.push(nd->topleft);
                    q.push(nd->topright);
                }
            }
        }
    }
}

RP::pqtree RP::QuadMapper::get_enclosing_node(point coord) const
{
    if (coord.x - 1e-6 > root->max_x || coord.x + 1e-6 < root->min_x || coord.y - 1e-6 > root->max_y ||
        coord.y + 1e-6 < root->min_y)
    {
        printf("WARNING: coord not enclosed in root node in QuadMapper.\n");
        return pqtree(nullptr);
    }

    pqtree cn = root;
    while (!cn->is_leaf)
    {
        if (cn->is_blocked)
        {
            printf("WARNING: coord enclosed in blocked node in QuadMapper.\n");
            return pqtree(nullptr);
        }
        if (coord.x < cn->center_coord.x)
        {
            if (coord.y < cn->center_coord.y)
                cn = cn->botleft;
            else
                cn = cn->topleft;
        }
        else
        {
            if (coord.y < cn->center_coord.y)
                cn = cn->botright;
            else
                cn = cn->topright;
        }
    }
    return cn;
}

RP::pqtree RP::QuadMapper::get_qtree_root() const
{
    return root;
}

static inline bool equal(float a, float b)
{
    return fabs(a - b) < 1e-5;
}

int RP::QuadMapper::qt2graph(pqtree qtn)
{
    qtn->graph_id = mygraph.create_node(qtn->center_coord);
    mygraph.nodes[qtn->graph_id].qt_id = qtn->qt_id;
    return qtn->graph_id;
}

void RP::QuadMapper::compute_graph()
{
    bool update = !new_nodes.empty();
    if (update)
    {
        int nedges_added = 0;
        for (int ni : new_nodes)
            qt2graph(qtnodes[ni]);
        size_t nqt = qtnodes.size();
        bool *visited = new bool[nqt]();
        for (int rm : removed_nodes)
        {
            pqtree rmd = qtnodes[rm];
            
            assert(rmd->graph_id > 1);
            const auto& conn = mygraph.nodes[rmd->graph_id].connection;
            // TODO make this code cleaner by removing neighbors AFTER
            // all edges are processed
            for (auto it = conn.begin(); it != conn.end();)
            {
                // re-connect old node's neighbors to new nodes
                int qtid = mygraph.nodes[it->second.child].qt_id;
                if (qtid == -1)
                {
                    assert(it->second.child == 0 || it->second.child == 1);
                    it++;
                    continue;
                }
                pqtree nb = qtnodes[qtid];
                if (nb->is_leaf && !nb->is_blocked)
                {
                    Direction dir;
                    if (equal(nb->max_x, rmd->min_x))
                    {
                        dir = RIGHT;
                    }
                    else if (equal(nb->min_x, rmd->max_x))
                    {
                        dir = LEFT;
                    }
                    else if (equal(nb->max_y, rmd->min_y))
                    {
                        dir = UP;
                    }
                    else
                    {
                        assert(equal(nb->min_y, rmd->max_y));
                        dir = DOWN;
                    }
                    pqtree newone = nb->get_neighbor_ge(dir);
                    // only add if new node is bigger than nb
                    if (newone && !newone->is_blocked && newone->depth < nb->depth)
                    {
                        assert(new_nodes.find(newone->qt_id) != new_nodes.end());
                        assert(newone->is_leaf);
                        nedges_added++;
                        assert(!newone->is_blocked);
                        mygraph.add_edge(newone->graph_id, nb->graph_id);
                        visited[nb->qt_id] = true;
                    }
                }
                // good night my sweet prince
                it = mygraph.remove_edge(it->second.parent, it->second.child);
            }
        }

        for (int ni : new_nodes)
        {
            if (qtnodes[ni]->is_blocked)
                continue;
            for (Direction d : {UP, DOWN, LEFT, RIGHT})
            {
                pqtree n = qtnodes[ni]->get_neighbor_ge(d);
                if (!n || !n->is_leaf || n->is_blocked)
                    continue;
                if (!visited[n->qt_id] || n->depth < qtnodes[ni]->depth)
                {
                    nedges_added++;
                    assert(!qtnodes[ni]->is_blocked);
                    assert(!n->is_blocked);
                    mygraph.add_edge(qtnodes[ni]->graph_id, n->graph_id);
                }
            }
        }
        new_nodes.clear();
        removed_nodes.clear();
    }

    if (cur_changed || update)
    {
        cur_changed = false;
        int removed = -1;
        if (!mygraph.nodes[0].connection.empty())
        {
            assert(mygraph.nodes[0].connection.size() == 1);
            removed = mygraph.nodes[0].connection.begin()->first;
            mygraph.remove_edge(0, mygraph.nodes[0].connection.begin()->first);
        }
        pqtree cur_node = get_enclosing_node(mygraph.nodes[0].coord);
        //printf("cur node %f, %f\n", cur_node->center_coord.x, cur_node->center_coord.y);
        if (cur_node)
            mygraph.add_edge(0, cur_node->graph_id);
        else
        {
            printf("%d\n", removed);
        }
        
    }

    if (tar_changed || update)
    {
        tar_changed = false;
        if (!mygraph.nodes[1].connection.empty())
        {
            assert(mygraph.nodes[1].connection.size() == 1);
            mygraph.remove_edge(1, mygraph.nodes[1].connection.begin()->first);
        }
        pqtree tar_node = get_enclosing_node(mygraph.nodes[1].coord);
        if (tar_node)
            mygraph.add_edge(1, tar_node->graph_id);
    }
}

// TODO add some tolerance to this in case robot runs into a blocked node
bool RP::QuadMapper::path_good(int node1, int node2, float tol) const
{
    line path_seg{mygraph.nodes[node1].coord, mygraph.nodes[node2].coord};
    point dont_care;
    std::queue<pqtree> q;
    q.push(root);
    while (!q.empty())
    {
        pqtree nd = q.front();
        q.pop();
        if (seg_intersects_rect(path_seg, nd->sides, dont_care))
        {
            if (nd->is_blocked)
                return false;
            if (!nd->is_leaf)
            {
                q.push(nd->topleft);
                q.push(nd->topright);
                q.push(nd->botleft);
                q.push(nd->botright);
            }
        }
    }
    return true;
}
