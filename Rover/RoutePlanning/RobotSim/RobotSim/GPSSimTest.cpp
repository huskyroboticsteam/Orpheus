#include "GPSSim.hpp"
#include <iostream>
#include "autonomous/utils.hpp"

int main() 
{
    RP::GPSSim sim;
    for (int i = 0; i < 3; i++) 
    {
        RP::point pt = sim.generate_pt(47.15 * 1000, 38.3730, 110.7140);
        std::cout << pt.x << ", " << pt.y << "\n" << std::endl;
    }
    return 0;
}

