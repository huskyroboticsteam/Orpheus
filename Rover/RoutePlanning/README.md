# Route Planning
Create the program for the robot on the Autonomous Traversal mission.

## Current Roles
* Tadeusz - UI
* Bryan - Final scanning of tennis ball
* Gary - Simulations
* Spencer - Robot pathmaking and maneuvering

## Overall Strategy
1. Start by scanning environment and attempting to identify ball
2. If not found, drive in a straight line towards target coords
3. If there's an obstacle in the way, drive in straight line which dodges obstacle
4. Once the bot's gone an acceptable distance, rescan + repeat
5. When ball is identified, drive to it

## Notes from Game Manual
* Ball is 10-50 cm above ground level
* Finish of one leg = start of next
* Teleop scouting allowed in initial stages, but must have full auto to path to marker
* Robot must say when it's within 2m of target w/ visual signal on rover
* Allowed to use teleop in between stages to "reset" bot
