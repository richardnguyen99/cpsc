#include <iostream>

#include "GridFlea.h"
#include "InFest.h"

int _main(int argc, char** argv)
{
    GridFlea flea1 = GridFlea(8, 8, 8);
    GridFlea flea2;
    GridFlea flea3;
    GridFlea flea4;

    flea2 = flea1 + 1;
    flea2 += 1;

    flea3 = ++flea2;
    flea4 = flea3++;

    //bool res1 = flea4 == flea2;

    InFest inFest1 = InFest(8, 8, 8, 10);
    InFest inFest2 = inFest1;
    inFest1 = inFest1 + flea2;
    inFest1 += flea3;

    inFest2 += flea4;

    return 0;
}
