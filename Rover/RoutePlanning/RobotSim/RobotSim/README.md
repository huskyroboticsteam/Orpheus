# Robot Simulation
## Obstacle resolution notes

* There's floating point tolerances scattered across the code otherwise the code won't work
* Obstacle resolution currently does not work with intersecting obstacles (though obstacles can share vertices).
Just split two obstacles into four if they are intersecting (even if a point of one obstacle is on another obstacle).