/**
 * @file InFest.cpp
 *
 * Implementation invariants - for class designer
 *
 * _gridfleas: Heap memory as the storage of GridFlea instances
 * _num: Number of the GridFlea instances in the InFest
 * _inactive_num: Number of the inactive GridFlea instances in the InFest
 *
 * - Constructor
 *  1. InFest(size_type x, size_t y, size_t gridflea_size, size_t gridflea_num)
 *      client will specify initial (x,y) for the first GridFlea object.
 *      Then, other by ten after every object is created. The client
 *      also needs to pass the GRIDFLEA_SIZE for consistent size of
 *      GridFlea objects, and GRIDFLEA_NUM for the number of GridFlea
 *      objects in the InFest. _gridfleas will be allocated based on GRIDFLEA_NUM.
 *
 *  2. InFest(const InFest &inFest)
 *      Copy constructor. The InFest will copy the content in other InFest,
 *      including _num, _inactive_num, without being compromised later. That's
 *      why const is marked the parameter list.
 *
 *  3. InFest(InFest &&inFest)
 *      Move constructor. Other InFest will transfer the ownership of
 *      its _gridfeals array to the new one. So that, no new allocation or
 *      deallocation is needed. Other private properties will be copied as well.

 *
 * - Assignment operator
 *  1. operator=(const InFest &inFest)
 *      Same as copy constructor. Additionally, it checks if both are the same
 *      and, deallocates an existing array of GridFleas before do the copying.
 *  2. operator=(InFest &&inFest)
 *      Same as move constructor. Additionally, it checks if both are the same
 *      and, deallocates an existing array of GridFleas before do the moving.
 *
 *      Every assignment operator returns the current instance (i.e. *this)
 *
 * - move(int p):
 *      p should follow what p in GridFlea::move(p) does. To make InFest respond
 *      consistently, after GridFlea::move() is called, check GridFlea::value()
 *      to keep track how many object has gone inactive (i.e. _inactive_num).
 *      If half of them are inactive, call revive() immediately to revive all
 *      inactive fleas. Noted that revive() only succeeds if the gridFlea is
 *      valid, which means still inside the grid
 *
 * - min() and max():
 *      min() and max() should return -2 when there is no elements or _gridflea
 *      array is invalid. Otherwise, it should return the smallest and biggest
 *      value based on GridFlea::value() accordingly.
 *
 *      The reason why is -2 is -1 is preserved in GridFlea to indicate inactive
 *      GridFleas. -2 hereby means no element in _gridfleas array.
 *
 */

#include "InFest.h"

InFest::InFest(size_type x, size_t y, size_t gridflea_size, size_t gridflea_num) : _num(gridflea_num), _inactive_num(0)
{
    this->_gridfleas = (GridFlea *)malloc(sizeof(GridFlea) * this->_num);

    int step = 0;
    for (size_type i = 0; i < this->_num; i++, step += 10)
    {
        this->_gridfleas[i] = GridFlea(x, y + step, gridflea_size);
    }
}

InFest::InFest(const InFest &inFest) : _num(inFest._num), _inactive_num(inFest._inactive_num)
{
    this->_gridfleas = new GridFlea[this->_num];

    for (size_type i = 0; i < this->_num; i++)
    {
        this->_gridfleas[i] = GridFlea(inFest._gridfleas[i]);
    }
}

InFest::InFest(InFest &&inFest) : _num(inFest._num), _inactive_num(inFest._inactive_num)
{
    this->_gridfleas = inFest._gridfleas;
    inFest._gridfleas = nullptr;
}

/**
 * Pre-conditions:
 *
 * InFest must be different
 *
 * Post-conditions:
 *
 * The content in _gridfleas might be changed
 *
 */
InFest &InFest::operator=(const InFest &inFest)
{
    if (this == &inFest)
        return *this;

    if (this->_gridfleas != nullptr)
    {
        delete[] this->_gridfleas;
        this->_gridfleas = nullptr;
    }

    this->_num = inFest._num;

    this->_gridfleas = (GridFlea *)malloc(sizeof(GridFlea) * this->_num);
    for (size_type i = 0; i < this->_num; i++)
    {
        this->_gridfleas[i] = GridFlea(inFest._gridfleas[i]);
    }

    return *this;
}

/**
 * Pre-conditions:
 *
 * InFest must be different
 *
 * Post-conditions:
 *
 * The contents in _gridfeals' might be changed
 */
InFest &InFest::operator=(InFest &&inFest)
{
    if (this == &inFest)
        return *this;

    if (this->_gridfleas != nullptr)
    {
        delete[] this->_gridfleas;
        this->_gridfleas = nullptr;
    }

    this->_num = inFest._num;
    this->_inactive_num = 0;

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
}

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 *
 */
void InFest::move(int p)
{
    if (this->_gridfleas == nullptr)
        return;

    for (size_type i = 0; i < this->_num; i++)
    {
        this->_gridfleas[i].move(p);

        if (this->_gridfleas[i].value() == -1)
            this->_inactive_num++;
    }

    if (this->_inactive_num > (this->_num / 2))
    {
        for (size_type i = 0; i < this->_num; i++)
            this->_gridfleas[i].revive();
    }
}

int InFest::min()
{
    if (this->_num == 0 || !this->_gridfleas)
        return -2;

    if (this->_num == 1)
        return this->_gridfleas[0].value();

    int min = this->_gridfleas[0].value();
    for (size_type i = 1; i < this->_num; i++)
    {
        if (min > this->_gridfleas[i].value())
            min = this->_gridfleas[i].value();
    }

    return min;
}

int InFest::max()
{
    if (this->_num == 0 || !this->_gridfleas)
        return -2;

    if (this->_num == 1)
        return this->_gridfleas[0].value();

    int max = this->_gridfleas[0].value();
    for (size_type i = 1; i < this->_num; i++)
    {
        if (max < this->_gridfleas[i].value())
            max = this->_gridfleas[i].value();
    }

    return max;
}
