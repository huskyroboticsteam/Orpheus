test: test.o MFile.o
	g++ -o $@ $+ -lkalman -L../../

test.o: test.cpp MFile.h
	g++ -I../../ -c -g -o $@ $< 

MFile.o: MFile.cpp MFile.h
	g++ -I../../ -c -g -o  $@ $<

