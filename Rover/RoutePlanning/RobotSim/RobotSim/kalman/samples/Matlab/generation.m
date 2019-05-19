%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Filename: generation.m
% Authors: Sylvain Marleau, Vincent Zalzal
% Description:  This script simulates the model and it generate mesures for
%               the matlab Kalman filter example.
%
% A plane flights in a 2D space where the x axis is the distance traveled
% by the plane and y axis is its altitude.  This system can be represented
% by the fallowing equations:
% (This is just an example)
%
% xpp = F/m - bx/m * xp^2
% ypp = p/m * xp^2 - g
%
% where m is the plane's weight (1000 kg)
%       bx is the drag coefficient (0.35 N/m²/s²)
%       p is the lift force (3.92 N/m²/s²)
%       g is the gravitational acceleration (9.8 m/s²)
%       F is the motor's thrust
%
% A station on the ground (at the origin) mesures the angle between the
% plane and the ground (x axis) and the distance between the plane and the station.
% These measures are based and the fallowing equations:
%
% theta = atan2(y,x)
% r = sqrt(x^2+y^2)
%
% The variance error matrix of the mesures is:
%
% R = [0.01^2  0
%      0       50^2]
%
% The variance error matrix of the plane's model is: WQW'
%
% Q = [0.01^2    0;
%      0         0.01^2];
%
% W = [0 0;
%      1 0;
%      0 0;
%      0 1];
%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

clear;
global T m b p g w1 w2

% Nb samples and sampling periode
N = 500;
T = 0.2;

% Parameters
m = 1000;
b = 0.35;
p = 3.92;
g = 9.8;

% noises
bt = 0.01;      
bd = 50;        
w1 = 0.01;
w2 = 0.01;

% time vector generation
time = zeros(1,N);
for t=1:N
    time(t) = t*T;
end

% generation of the plane thrust vector
F = zeros(1,N);
for t=1:N
    F(t) = 875 + 5*sin(t/N*2*2*pi);
end

% Initiales conditions [x vx y vy]
q0 = [-2500; 50; 250; 0]

% Simulation and generation of the real plane trajectory
trajectory = zeros(4,N);
trajectory(:,1) = q0;
for t=2:N
    trajectory(:,t) = model(trajectory(:,t-1), F(t));
end

% Generation of measures
measures = zeros(2,N);
for t=1:N
    measures(1,t) = atan2(trajectory(3,t), trajectory(1,t)) + random('Normal', 0, bt);
    measures(2,t) = sqrt(trajectory(1,t)^2 + trajectory(3,t)^2) + random('Normal', 0, bd);
end 

% Generation of the measured trajectory
trajectory_measure = zeros(4,N);
for t=1:N
   trajectory_measure(1,t) = cos(measures(1,t))*measures(2,t);
   trajectory_measure(3,t) = sin(measures(1,t))*measures(2,t);
end

for t=2:N
   trajectory_measure(2,t) = (trajectory_measure(1,t) - trajectory_measure(1,t-1))/T;
   trajectory_measure(4,t) = (trajectory_measure(3,t) - trajectory_measure(3,t-1))/T;
end

trajectory_measure(2,1) = trajectory_measure(2,2);
trajectory_measure(4,1) = trajectory_measure(4,2);

figure(1);
plot(trajectory(1,:), trajectory(3,:), trajectory_measure(1,:), trajectory_measure(3,:));
title('Real and measured trajectory');
axis([-2500 2500 0 400]);

figure(2);
plot(time, trajectory(2,:), time, trajectory(4,:), time, trajectory_measure(2,:), time, trajectory_measure(4,:));
title('Real and measured plane''s speed');

figure(3);
plot(time, F);
title('Thrust');

%save data
save data.mat time F measures trajectory trajectory_measure

fid = fopen('data.m', 'w');

exportMatlab(fid, 'F', F);
exportMatlab(fid, 'measures', measures);

fclose(fid);
