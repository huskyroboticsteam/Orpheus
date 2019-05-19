%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Filename: kalman.m
% Authors: Sylvain Marleau, Vincent Zalzal
% Description:  This script is a matlab Kalman filter example based on the
%               article : Welch Bishop, An Introduction to the Kalman Filter, 
%               University of North Carolina at Chapel Hill, May 2003.
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

clear;
global T m b p g w1 w2

% loading thrust and measures
data

% Parameters
N = 500;
T = 0.2;
m = 1000;
b = 0.35;
p = 3.92;
g = 9.8;
w1 = 0.0;
w2 = 0.0;

Q = [0.01^2          0.01*0.01/10;
     0.01*0.01/10    0.01^2];

W = [0 0;
     1 0;
     0 0;
     0 1];

V = [1 0;
     0 1];
 
R = [0.01^2          0.0;
     0.0      50^2];

% initial estimate
x = [cos(measures(1,1))*measures(2,1); 60; sin(measures(1,1))*measures(2,1); 0];

% estimé de l'erreur initiale
P = [100^2     0       0       0;
     0         10^2    0      0;
     0         0       25^2    0;
     0         0       0       10^2];

trajectory_kalman = zeros(4,N);
trajectory_kalman(:,1) = x;

for t=2:N

% Prediction
x_ = model(x,F(t));

A = createA(x);
H = createH(x_);

% Error prediction 
P_ = A*P*A' + W*Q*W';

% Kalman's gain
K = P_*H'/(H*P_*H'+V*R*V');

% measures estimate
estime = [atan2(x_(3), x_(1)); sqrt(x_(1)^2+x_(3)^2)];

correction = K*(measures(:,t) - estime);

% New estimate of state vector
x = x_ + correction;

% Error estimate
P = (eye(4) - K*H)*P_;

trajectory_kalman(:,t) = x;
end

fid = fopen('trajectory_matlab.m', 'w');

exportMatlab(fid, 'trajectory_kalman', trajectory_kalman);

fclose(fid);
