# Define compiler options
CXX=g++
FLAGS=-g -Wall -Werror -std=c++17

SRC := driver.cpp GridFlea.cpp InFest.cpp
HDR := GridFlea.h InFest.h
OBJ := $(patsubst %.cpp, %.o, $(SRC))

all: driver

driver: $(OBJ)
	$(CXX) -o $@ $(OBJ)

%.o: %.cpp $(HDR)
	$(CXX) $(FLAGS) -c -o $@ $<

tar:
	tar -cvf p4.tar *.cpp *.h Makefile

clean:
	rm -rf driver *.o *.tar
