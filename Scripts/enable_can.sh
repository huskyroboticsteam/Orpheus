#!/bin/bash

sudo modprobe can
sudo modprobe can-dev
sudo modprobe can-raw

sudo ip link set can0 up type can bitrate 125000
sudo ifconfig can0 txqueuelen 1000
sudo ifconfig can0 up
