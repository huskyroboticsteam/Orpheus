#include "Server.cpp"
#include <stdio.h>
#include <string>
#include <vector>
#include <sstream>
#include <iostream>

int main()
{
    RP::Server server1;

    std::cout << "Usage: \"Command\" or \"Quit\" \n";

    while (true)
    {
        std::cout << "Enter a command: ";
        std::string input;
        std::cin >> input;
        std::cout << "\n";

        if (!(input.compare("Quit") == 0))
        {
            if (input.compare("Command") == 0)
            {
                short speed;
                short heading;

                std::cout << "Enter Speed \n";
                std::cin >> speed;

                std::cout << "Enter Heading \n";
                std::cin >> heading;

                std::vector<unsigned char> data(5);

                data[0] = 0;
                memcpy(&data[1], &speed, sizeof speed);
                memcpy(&data[3], &heading, sizeof heading);

                server1.send_action(data);
            }
            else
            {
                std::cout << "Invalid Input";
            }
        }
        else
        {
            return 0;
        }
    }
    return 0;
}