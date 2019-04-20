#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <netdb.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netinet/in_systm.h>
#include <netinet/ip.h>
#include <netinet/tcp.h>
#include <arpa/inet.h>
 
 int main()
{
 
	int port = 1936;
	char addr[15] = "2.0.0.2";
	 
	// I know you intaililize the socket with socket
	Socket sock = socket (AF_INET, SOCK_STREAM, 0);
	 
	// This is where i get confused
	struct sockaddr_in addr;
	address.sin_family = AF_INET; //we're using inet
	address.sin_port = htons (port); //set the port
	 
	connect (sock, (struct sockaddr *) &address, sizeof(address))
	 
	//Create the packet
	sprintf(packet, "La la la la");
	 
	//And send it
	write (sock, packet, strlen(packet));
	close (sock); // Close the connection
 
}

/*#include <iostream>
using namespace std;

int main()
{
	io_service io_service;
	ip::udp::socket socket(io_service);
	ip::udp::endpoint remote_endpoint;
	socket.open(ip::udp::v4());
	
	remote_endpoint = ip::udp::endpoint(ip::address::from_string("192.168.0.20"), 9000);
	
	string Result = "";
	double direction = 0.0;
	ostringstream convert;
	convert << direction;
	Result = convert.str();

	string Result2 = "";
	double speed = 0.0;
	ostringstream convert2;	
	convert2 << speed;
	Result2 = convert2.str();      

    socket.send_to(buffer((Result2 + "," + Result), 16), remote_endpoint, 0, err);
}*/