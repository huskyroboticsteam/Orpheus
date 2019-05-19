%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Filename: result.m
% Authors: Sylvain Marleau, Vincent Zalzal
% Description:  This script shows filtered trajectories and compares the
%               common kalman filter with the UDU kalman filter.
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

clear;

% Load data
load data.mat
trajectory_matlab;
trajectory_udu_load;

figure(1);
plot(trajectory(1,:), trajectory(3,:), '--', trajectory_measure(1,:), trajectory_measure(3,:), ':', trajectory_kalman(1,:), trajectory_kalman(3,:), '-');
title('Real, Measured and Kalman Trajectories');
axis([-2500 2500 0 400]);
legend('Real', 'Measured', 'Kalman');
xlabel('Distance (m)');
ylabel('Altitude (m)');
print -f1 -deps2c position

figure(2);
plot(time, trajectory(2,:), '--', time, trajectory_measure(2,:), ':', time, trajectory_kalman(2,:), '-');
title('Ground speed');
legend('Real', 'Measured', 'Kalman');
xlabel('time (sec)');
ylabel('speed (m/s)');
print -f2 -deps2c speedX

figure(3);
plot(time, trajectory(4,:), '--', time, trajectory_measure(4,:), ':', time, trajectory_kalman(4,:), '-');
title('Elevation speed');
legend('Real', 'Measured', 'Kalman');
xlabel('time (sec)');
ylabel('speed (m/s)');
print -f3 -deps2c speedY

figure(4);
plot(time, trajectory_udu(2,:)-trajectory_kalman(2,:), '-', time, trajectory_udu(4,:)-trajectory_kalman(4,:), '--');
title('Speed comparison between common Kalman and UDU Kalman');
legend('x', 'y');
xlabel('time (sec)');
ylabel('speed (m/s)');
print -f4 -deps2c compareSpeed

figure(5);
plot(time, trajectory_udu(1,:)-trajectory_kalman(1,:), '-', time, trajectory_udu(3,:)-trajectory_kalman(3,:), '--');
title('Position comparison between common Kalman and UDU Kalman');
legend('x', 'y');
xlabel('time (sec)');
ylabel('position (m)');
print -f5 -deps2c comparePosition

