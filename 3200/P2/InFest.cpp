#include "InFest.h"
#include <cstdlib>
#include <memory>

InFest::InFest(size_t x, size_t y, size_t gridflea_size, size_t gridflea_num) : _num(gridflea_num)
{
    this->_gridfleas = (GridFlea *)malloc(sizeof(GridFlea) * this->_num);

    for (int i = 0; i < this->_num; i++)
    {
        this->_gridfleas[i] = GridFlea(x, y++, gridflea_size);
    }
}

InFest::InFest(const InFest &inFest) : _num(inFest._num)
{
    this->_gridfleas = (GridFlea *)malloc(sizeof(GridFlea) * this->_num);

    for (int i = 0; i < this->_num; i++)
    {
        this->_gridfleas[i] = GridFlea(inFest._gridfleas[i]);
    }
}

InFest::InFest(InFest &&inFest) : _num(inFest._num)
{
    this->_gridfleas = inFest._gridfleas;
    inFest._gridfleas = nullptr;
}

InFest &InFest::operator=(const InFest &inFest)
{
    if (this == &inFest)
        return *this;

    delete[] this->_gridfleas;

    this->_num = inFest._num;

    this->_gridfleas = (GridFlea *)malloc(sizeof(GridFlea) * this->_num);
    for (int i = 0; i < this->_num; i++)
    {
        this->_gridfleas[i] = GridFlea(inFest._gridfleas[i]);
    }

    return *this;
}

InFest &InFest::operator=(InFest &&inFest)
{
    if (this == &inFest)
        return *this;

    delete[] this->_gridfleas;
    this->_gridfleas = nullptr;

    this->_num = inFest._num;

    this->_gridfleas = (GridFlea *)malloc(sizeof(GridFlea) * this->_num);
    for (int i = 0; i < this->_num; i++)
    {
        this->_gridfleas[i] = std::move(inFest._gridfleas[i]);
    }

    return *this;
}

InFest::~InFest()
{
    if (this->_gridfleas == nullptr)
        return;

    this->_num = 0;

    delete[] this->_gridfleas;
    this->_gridfleas = nullptr;
}

int InFest::value()
{
    if (this->_gridfleas == nullptr)
        return -1;

    int sum = 0;
    for (int i = 0; i < this->_num; i++)
    {
        sum += this->_gridfleas[i].value();
    }

    return sum;
}

void InFest::move(int p)
{
    if (this->_gridfleas == nullptr)
        return;

    for (int i = 0; i < this->_num; i++)
    {
        this->_gridfleas[i].move(p);
    }
}
