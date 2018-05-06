#!/bin/bash

ip=$1
if [ ! -z "$ip" ]; then
ip_thing="\niface eth0 inet static\n
  address $1\n
  gateway 198.168.0.1"

echo $ip_thing >> /etc/network/interfaces
else
echo "IP Address must not be empty"
fi
