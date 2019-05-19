function H = createH(q)

global T m b p g w1 w2

H = [-q(3)/(q(1)^2+q(3)^2)      0        q(1)/(q(1)^2+q(3)^2)       0;
     q(1)/sqrt(q(1)^2+q(3)^2)   0        q(3)/sqrt(q(1)^2+q(3)^2)   0];
