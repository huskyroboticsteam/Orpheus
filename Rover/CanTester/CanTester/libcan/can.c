#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <stdint.h>
#include <errno.h>

#include <net/if.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/ioctl.h>

#include <linux/can.h>
#include <linux/can/raw.h>

int s[2];

int InitCan(int cannum)
{
    char *ifname;
    switch(cannum)
    {
    case 0:
	ifname = "can0";
    case 1:
	ifname = "can1";
    default:
	return(-1);       
    }

    struct ifreq ifr;
    struct sockaddr_can addr;
    if((s[cannum] = socket(PF_CAN, SOCK_RAW, CAN_RAW)) < 0)
	return(-1);

    strcpy(ifr.ifr_name, ifname);
    ioctl(s, SIOCGIFINDEX, &ifr);

    addr.can_family = AF_CAN;
    addr.can_ifindex = ifr.ifr_ifindex;

    if(bind(s[cannum], (struct sockaddr *)&addr, sizeof(addr)) < 0)
	return(-2);
	
    return(1);	
}

int Send(int id, uint8_t *payload, uint32_t len, int cannum)
{
    struct can_frame frame;
    frame.can_id = id;
    int nbytes = 0;
    while(len)
    {
	int size = 8;
	if(len < 8)
	    size = len;
	memcpy(frame.data, payload, size);
	frame.can_dlc = size;
	payload += size;
	len -= size;
	nbytes += write(s[cannum], &frame, sizeof(struct can_frame));		
    }
    if(nbytes < 0)
	nbytes = -1;
    return(nbytes);
}

struct can_frame Read(int cannum)
{
    struct can_frame frame;
    read(s[cannum], &frame, sizeof(struct can_frame));
    return(frame);
}

int Close(int cannum)
{
    close(s[cannum]);
}
