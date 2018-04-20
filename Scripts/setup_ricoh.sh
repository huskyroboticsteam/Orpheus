#!/bin/bash

# Expected that Ricoh Theta is powered on, plugged in, and not mounted as a live device
sudo apt-get install gphoto2
sudo gphoto2 --set-config d803=0 # Disable sleeping
sudo gphoto2 --set-config d802=0 # Disable auto power off

# Further documentation is found here: https://developers.theta360.com/en/docs/v2/usb_reference/
