#include <vector>
#include <sys/types.h>
//#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h> 
#include <errno.h>
#include <unistd.h>
#include <string.h>
#include <mutex>
#include <queue>
#include <thread>
#include <iostream>
#include "autonomous/utils.hpp"
#include "GPSSim.hpp"

#define SOCKET int
#define SOCKET_ERROR -1


class WorldCommunicator {
	public:
		// Sends and receives packets
		// To be called in the main update loop
		void update(const RP::point& position, const float& rotation, float& move, float& turn, const std::vector<RP::line>& obstacles);
		WorldCommunicator();
	private:
		int timer;
		bool send_action(std::vector<unsigned char> data, const unsigned char id);
		void listen();
		RP::GPSSim gpsSim;
		// Socket used to send stuff
		SOCKET out;
		// Socket used to receive stuff
		SOCKET in;
		//Mutex used to lock packetQ during multithreading
		std::mutex mtx;
		// Queue of packets to be processed
		std::queue<std::vector<unsigned char>> packetQ;
		// Thread used to run listen method asynchronously
		std::thread listenThread;
		// Address we should send stuff to
		sockaddr_in send_to;
		const int framesPerGPS = 10;
		const int framesPerMag = 5;
		unsigned const char gpsId = 10;
		unsigned const char magId = 11;
		const float magError = 1.8;
};
