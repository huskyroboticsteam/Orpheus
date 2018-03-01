# Base Station
Contains the code that will run on the base station computer, mainly the user interface,
along with the server code that will communicate with rover to control it and receive 
information from it.

## Project Layout
The BaseStation project contains the entry point of the base station in the
HuskyRobotics.BaseStation.StartBaseStation class. The user interface
components are contained in the UserInterface project. The VideoStreamer
project is for video streaming UI elements. Utilities contains methods
for use by all base station projects. The BaseServer project is for
server logic, but may be merged with the BaseStation project.

## UI Functions
 - Navigation
 - Diagnostics
 - Video Feed

## Orientation/Informational Map
 - Show current position and direction of rover
 - Click on map to select a point
 - Be able to type in coordinates and show them
 - Points are selectable
 - Unload map tiles that aren't in the viewport
 - Distance and direction between waypoints
 - List of waypoints
 - Named waypoints
 - Goal waypoint
 
## Status Info UI
 - Wheel spinning/stuck status
 - Direction of wheels
 - See the encoders, BE the encoders
 - Same with arm
 
## Video Feeds
 - separate windows
 - Always on top option
 
## Required Libraries
 - GStreamer must be installed for VideoStreamer to work
