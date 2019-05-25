#include "Server.cpp"

int main() {
	RP::Server server1 = RP::Server();
	server1.send_action(0x02);
	return 0;
}