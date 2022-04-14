#include "InFest.h"
#include <cstdlib>
#include <memory>

InFest::InFest(size_t x, size_t y, size_t gridflea_size, size_t gridflea_num) : _num(gridflea_num)
{
    this->_gridfleas = (GridFlea *)malloc(sizeof(GridFlea) * this->_num);

    int step = 10;
    for (size_t i = 0; i < this->_num; i++, step += 10)
    {
        this->_gridfleas[i] = GridFlea(x, y + step, gridflea_size);
    }
}

InFest::InFest(const InFest &inFest) : _num(inFest._num)
{
    this->_gridfleas = (GridFlea *)malloc(sizeof(GridFlea) * this->_num);

    for (size_t i = 0; i < this->_num; i++)
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

    if (this->_gridfleas != nullptr)
        delete[] this->_gridfleas;

    this->_num = inFest._num;

    this->_gridfleas = (GridFlea *)malloc(sizeof(GridFlea) * this->_num);
    for (size_t i = 0; i < this->_num; i++)
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

    this->_gridfleas = inFest._gridfleas;
    inFest._gridfleas = nullptr;

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
    for (size_t i = 0; i < this->_num; i++)
    {
        sum += this->_gridfleas[i].value();
    }

    return sum;
}

void InFest::move(int p)
{
    if (this->_gridfleas == nullptr)
        return;

    for (size_t i = 0; i < this->_num; i++)
    {
        this->_gridfleas[i].move(p);
    }
}

int InFest::min()
{
    if (this->_num == 0)
        return -1;

    if (this->_num == 1)
        return this->_gridfleas[0].value();

    int min = this->_gridfleas[0].value();
    for (size_t i = 1; i < this->_num; i++)
    {
        if (min > this->_gridfleas[i].value())
            min = this->_gridfleas[i].value();
    }

    return min;
}

int InFest::max()
{
    if (this->_num == 0)
        return -1;

    if (this->_num == 1)
        return this->_gridfleas[0].value();

    int max = this->_gridfleas[0].value();
    for (size_t i = 1; i < this->_num; i++)
    {
        if (max < this->_gridfleas[i].value())
            max = this->_gridfleas[i].value();
    }

    return max;
}
