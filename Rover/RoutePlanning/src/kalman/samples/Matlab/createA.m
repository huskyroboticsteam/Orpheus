function A = createA(q)

global T m b p g w1 w2

A = [1    T-T^2*b/m*q(2)      0        0;
     0    1-2*T*b/m*q(2)      0        0;
     0    T^2*p/m*q(2)        1        T;
     0    2*T*p/m*q(2)        0        1];
