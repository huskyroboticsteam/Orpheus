#include "Server.cpp"
#include <stdio.h>
#include <string>
#include <vector>
#include <sstream>
#include <iostream>

std::vector<std::string> split(std::string str, char delimiter) {
  std::vector<std::string> internal;
  std::stringstream ss(str); // Turn the string into a stream.
  std::string tok;
 
  while(getline(ss, tok, delimiter)) {
    internal.push_back(tok);
  }
 
  return internal;
}

int main() {
    RP::Server server1 = RP::Server();

    std::cout << "Usage Ex: \"move,50\" will set movement power to 50. \"turn,30\" will turn the rover 30 degrees \n";

    while(true) {
        std::cout << "Enter a command: ";
        std::string input;
        std::cin >> input;
        std::cout << "\n";

        std::vector<std::string> splitInput = split(input, ',');

        if(splitInput.size() != 2) 
        {
            std::cout << "Invalid Input Argument Count of " << splitInput.size() << " \n";
        } 
        else {
            std::string command = splitInput.at(0);
            std::string dataString = splitInput.at(1);
            float dataFloat = std::stof(dataString);

            unsigned char data[sizeof(float)];
            memcpy(data, &dataFloat, sizeof dataFloat);
            std::vector<unsigned char> dataVector = std::vector<unsigned char>(data, data + sizeof data / sizeof data[0]);

            if(command.compare("move")) 
            {
                std::cout << "Moving" << dataString << "\n";
                server1.send_action(dataVector, 0x1);
            } 
            else if(command.compare("turn")) 
            {
                std::cout << "Turning" << dataString << "\n";
                server1.send_action(dataVector, 0x2);
            }
            else if (command.compare("quit"))
            {
                std::cout << "Quitting \n";
                return 0;
            }
            else {
                std::cout << "Invalid Input";
            }
        }
    }
	
	return 0;
}