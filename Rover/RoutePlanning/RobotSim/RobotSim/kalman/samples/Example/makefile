example : example.o plane.o ../../kstatics.o
	g++ -g -o $@ $+ -Wno-deprecated -lkalman -L../../ 

example.o : example.cpp plane.h
	g++ -g -c -o $@ $< -I../../kalman -I../../

plane.o : plane.cpp plane.h
	g++ -g -c -o $@ $< -I../../kalman -I../../

.PHONY : clean

clean :
	rm -f example;
	rm -f *.o
