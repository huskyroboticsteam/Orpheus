#include "Server.hpp"
#include <thread>
#include <chrono>

int main() {
    RP::Server server;
    while(true) {
        server.send_watchdog();
        std::cout << "sent" << std::endl;
    }
}