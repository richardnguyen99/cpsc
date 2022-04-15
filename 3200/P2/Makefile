# Define compiler options
CXX=g++
FLAGS=-g -Wall -Werror -std=c++17

SRC := main.cpp GridFlea.cpp InFest.cpp
HDR := GridFlea.h InFest.h
OBJ := $(patsubst %.cpp, %.o, $(SRC))

all: main

main: $(OBJ)
	$(CXX) -o $@ $(OBJ)

%.o: %.cpp $(HDR)
	$(CXX) $(FLAGS) -c -o $@ $<

tar:
	tar -cvf p2.tar *.cpp *.h Makefile

clean:
	rm -rf main *.o *.tar
