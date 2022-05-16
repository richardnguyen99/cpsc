#include <iostream>

#include "GridFlea.h"

int main(int argc, char** argv)
{
    GridFlea flea1 = GridFlea(8, 8, 8);
    GridFlea flea2;
    GridFlea flea3;
    GridFlea flea4;

    flea2 = flea1 + 1;
    flea2 += 1;

    flea3 = ++flea2;
    flea4 = flea3++;

    return 0;
}
