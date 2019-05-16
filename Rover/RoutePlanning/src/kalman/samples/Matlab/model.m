function n_q = model(q, F)

global T m b p g w1 w2

n_q = [q(1) + q(2)*T + T^2/2*(F/m - b/m*q(2)^2); 
       q(2) + (F/m - b/m*q(2)^2)*T + random('Normal', 0 ,w1); 
       q(3) + q(4)*T + T^2/2*(p/m*q(2)^2 - g); 
       q(4) + (p/m*q(2)^2 - g)*T + random('Normal', 0 ,w2)];
