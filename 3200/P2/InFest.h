#ifndef __INFEST_H
#define __INFEST_H 1

#include <cstddef>

#include "GridFlea.h"

class InFest
{
private:
    using size_t = std::size_t;

    size_t _num;
    GridFlea *_gridfleas;

public:
    InFest(size_t x, size_t y, size_t gridflea_size, size_t gridflea_num);
    InFest(const InFest &inFest);
    InFest(InFest &&inFest);
    ~InFest();

    InFest &operator=(const InFest &inFest);
    InFest &operator=(InFest &&inFest);

    int value();
    void move(int p);
};

#endif // __INFREST_H
