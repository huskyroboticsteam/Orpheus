/*
Server class that handles networking; e.g. setting up a server and connections,
sending/receiving and encoding/decoding data packets, etc.
*/

#ifndef ROVERPATHFINDING_SERVER_H
#define ROVERPATHFINDING_SERVER_H

#include <vector>
#include <mutex>
#include <ctime>
#include <iostream>

namespace RP
{
class Server
{
public:
	Server();
	// TODO add parameters that encapsulate the action
	bool send_action(std::vector<unsigned char> data);									 //send data in a packet without extra info
	bool send_action(std::vector<unsigned char> data, unsigned char id); // Sends action to client with data body, returns whether action was successful or not
	bool send_action(unsigned char data, unsigned char id);							 // Sends action to client with data body, returns whether action was successful or not
	bool send_action(unsigned char id);																	 // Sends action to client without data body, returns whether action was successful or not
	void send_watchdog();																								 // Sends watchdog every 100ms so this client isn't kicked out
	void data_receiver_loop();
	void stop(); // Stops socket and cleans up
	//RP::Controller controller;
	void get_packet_data(char *output);

private:
	std::vector<unsigned char> current_time(); // Stores unix timestamp in 4 bytes
	char packet_buf[buf_size];
	std::mutex buf_mutex;
	// RP::Controller controller;
};
} // namespace RP

#endif
