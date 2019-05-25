// file for memorizing obstacles
#ifndef RP_MEMORIZER_HPP
#define RP_MEMORIZER_HPP

#include <vector>

#include "utils.hpp"

namespace RP
{
class Memorizer
{
    public:
    void add_obstacles(const std::vector<line>& new_obstacles);
    const std::vector<line>& obstacles_ref = obstacles;
    private:
    std::vector<line> obstacles;
    line merge(const line&o, const line& p, bool& can_merge); 
};
} // namespace RP
#endif