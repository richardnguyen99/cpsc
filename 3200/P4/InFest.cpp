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
 *
 *  2. operator=(InFest &&inFest)
 *      Same as move constructor. Additionally, it checks if both are the same
 *      and, deallocates an existing array of GridFleas before do the moving.
 *
 *      Every assignment operator returns the current instance (i.e. *this)
 *
 *  3. operator+=(const GridFlea& gridflea)
 *     Allow appending a new GridFlea object. Since InFest supports assign and
 *     plus operator to append a new gridflea and return a new InFest, this is
 *     a shortcut to compact them without copying.
 *
 *  4. operator+=(int p)
 *     Allow moving the InFest. Since InFest supports assign and plus operator
 *     to move and create a new InFest, this is a shortcut to compact them
 *     without copying
 *
 * - Addition & subtraction overloaded operators:
 *  1. Unary incrementation and decrementaion: operator++(), operator--()
 *      Allow simplified move with one step. Postfix unary operations will return
 *      a copy of the old (before-moved) InFest without compromising the original
 *      one while the prefix ones will directly return the original InFest. In
 *      both scenarios, the original one will get modified by calling InFest::move()
 *
 *  2. Binary addition and subtraction: operator+(), operator-()
 *      Allow simplied move with `p` steps. These operations simply call InFest::move()
 *      to move the InFest and return a copy of it before moving. Since they guarantee
 *      there is no modification on the original InFest, they are marked as const.
 *
 * 3. Binary addition with GridFlea: operator+(const GridFlea& gridflea)
 *      Allow appending a new GridFlea to the InFest. A private constructor will be
 *      called to copy the new size and allocate heap blocks according to this size.
 *      Then, it simply copy the old InFest to the new one as well as append the new
 *      GridFlea to it. Basically, it's a deep copying with a twist so that it doesn't
 *      incur copying and reallocating at different times.
 *
 * - Comparison operators:
 *      Overloaded comparison operators are marked with const and take const reference
 *      object to ensure that first, they don't change both InFest objects used in these
 *      operations and second, they avoid deep copying on non-modification methods.
 *
 *      Only operator==, operator< incur value(). Other are associating with these two.
 *
 * - move(int p):
 *      p should follow what p in GridFlea::move(p) does. To make InFest respond
 *      consistently, after GridFlea::move() is called, check GridFlea::value()
 *      to keep track how many object has gone inactive (i.e. _inactive_num).
 *      If half of them are inactive, call revive() immediately to revive all
 *      inactive fleas. Noted that revive() only succeeds if the gridFlea is
 *      valid, which means still inside the grid
 *
 * - value():
 *      value should only add up the values that can be computed successfully from
 *      GridFlea::value(), aka active grid flea. So if the return value from this
 *      function decreases, it means there is at least one grid flea going inactive.
 *      If an InFest has no GridFlea object, it returns -2. If an InFest has GridFlea
 *      objects that are all inactive, it returns -1. It's because of distinguishing
 *      between active gridfleas that has not yet moved and inactive ones.
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

InFest::InFest(size_type x, size_type y, size_type gridflea_size, size_type gridflea_num) : _num(gridflea_num), _inactive_num(0)
{
    this->_gridfleas = (GridFlea *)malloc(sizeof(GridFlea) * this->_num);

    int step = 0;
    for (size_type i = 0; i < this->_num; i++, step += 10)
    {
        this->_gridfleas[i] = GridFlea(x, y + step, gridflea_size);
    }
}

InFest::InFest(size_type num) : _num(num), _inactive_num(0)
{
    this->_gridfleas = (GridFlea *)malloc(sizeof(GridFlea) * this->_num);
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
 * The contents in _gridfleas' might be changed
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

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 */
InFest InFest::operator+(int p) const
{
    InFest tmp = *this;
    tmp.move(p);
    return tmp;
}

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 */
InFest& InFest::operator+=(int p)
{
    this->move(p);
    return *this;
}

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 */
InFest InFest::operator-(int p) const
{
    InFest tmp = *this;
    tmp.move(-p);
    return tmp;
}

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 */
InFest& InFest::operator-=(int p)
{
    this->move(-p);
    return *this;
}

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 */
InFest& InFest::operator++()
{
    this->move(1);
    return *this;
}

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 */
InFest InFest::operator++(int)
{
    InFest tmp = *this;
    this->move(1);
    return tmp;
}

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 */
InFest& InFest::operator--()
{
    this->move(-1);
    return *this;
}

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 */
InFest InFest::operator--(int)
{
    InFest tmp = *this;
    this->move(-1);
    return tmp;
}

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 */
InFest InFest::operator+(const GridFlea& gridFlea)
{
    InFest newInFest(this->_num + 1);

    newInFest._inactive_num = this->_inactive_num;

    for (size_type i = 0; i < this->_num; i++)
        newInFest._gridfleas[i] = this->_gridfleas[i];

    newInFest._gridfleas[this->_num] = gridFlea;
    if (gridFlea.value() != -1)
        newInFest._inactive_num++;

    return newInFest;
}

InFest& InFest::operator+=(const GridFlea& gridFlea)
{
    this->_num++;
    this->_gridfleas = (GridFlea *)realloc(this->_gridfleas, sizeof(GridFlea) * this->_num);
    this->_gridfleas[this->_num-1] = gridFlea;

    if (gridFlea.value() == -1)
        this->_inactive_num++;

    return *this;
}

bool InFest::operator==(const InFest& other) const
{
    return this->value() == other.value();
}

bool InFest::operator<(const InFest& other) const
{
    return this->value() < other.value();
}

bool InFest::operator!=(const InFest& other) const
{
    return !(*this == other);
}

bool InFest::operator>(const InFest& other) const
{
    return other < *this;
}

bool InFest::operator>=(const InFest& other) const
{
    return !(*this < other);
}

bool InFest::operator<=(const InFest& other) const
{
    return !(*this > other);
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

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 */
int InFest::min() const
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

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 */
int InFest::max() const
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

/**
 * Pre-conditions:
 *
 * Infest must be non-empty
 *
 */
int InFest::value() const
{
    if (this->_num == 0 || !this->_gridfleas)
        return -2;

    int sum = 0;
    for (size_type i = 0; i < this->_num; i++)
    {
        if (this->_gridfleas[i].value() != -1)
            sum += this->_gridfleas[i].value();
    }

    return sum;
}

int InFest::size() const
{
    return this->_num;
}
