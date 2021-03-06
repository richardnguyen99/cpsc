/**
 * @file GridFlea.cpp
 * @version 0.2
 *
 * Implementation invariants - For class designer
 *
 * INITIAL_X and INITIAL_Y: predefined internally for consistency and easy testing
 * REWARD: factor to calculate value(). move properties must be based on this.
 *
 * _current_x and _current_y are changed upon move()
 * _max_active_move and _max_energetic_move are calculated based on size
 *      The result should be always less than the constant REWARD so that value()
 *      cannot yield to negative result. To ensure that, a modulus size of REWARD
 *      is performed. It's determined
 * _used_move are increased upon the result steps moving after move() is called.
 *      It will be set to 0 whenever revive() or reset() is called.
 *
 * _active is calculated based on the comparison (_used_move < _max_active_move)
 *      A permanently inactive, on the other hand, is determined based on wheter
 *      it has gone outside of the grid boundary or not.
 *
 * _energetic is calculated based on the comparison (_used_move < _max_energetic_move)
 *
 *
 * - GridFlea() and GridFlea(size_t, size_t y, size_t size)
 *      must call _initialize_gridflea(x,y) to handle client's input for
 *      position so that every GridFlea is not outside of the grid at first

 * - operator+=(int p) & operator-=(int p)
 *     Allow moving the GridFlea. Since GridFlea supports assign and plus operator
 *     to move and create a new GridFlea, this is a shortcut to compact them
 *     without copying.
 *
 * - Addition & subtraction overloaded operators:
 *  1. Unary incrementation and decrementaion: operator++(), operator--()
 *      Allow simplified move with one step. Postfix unary operations will return
 *      a copy of the old (before-moved) GridFlea without compromising the original
 *      one while the prefix ones will directly return the original GridFlea. In
 *      both scenarios, the original one will get modified by calling ::move()
 *
 *  2. Binary addition and subtraction: operator+(), operator-()
 *      Allow simplied move with `p` steps. These operations simply call GridFlea::move()
 *      to move the GridFlea and return a copy of it before moving. Since they guarantee
 *      there is no modification on the original GridFlea, they are marked as const.
 *
 *
 * - Comparison operators:
 *      Overloaded comparison operators are marked with const and take const reference
 *      object to ensure that first, they don't change both GridFlea objects used in these
 *      operations and second, they avoid deep copying on non-modification methods.
 *
 *      Only operator==, operator< incur value(). Other are associating with these two.
 *
 * - move(int p)
 *      must divide p by 2 if p is even is the actual step to move. _current_x
 *      and _current_y should be fine to work with signed values since move()
 *      accepts moving in both direction on the same axis. However, _used_move
 *      must work with unsigned value to calculate the number steps has been
 *      moved, not the displacement like _current_x and _current_y.
 *      In non-energetic mode, no division is needed since it only moves by one.
 *      But other statements must be held true.
 *
 * - value()
 *      cares how far the gridflea is from its initial location. So it doesn't
 *      care what direction, or where the coordinates are. To ensure the absolute
 *      distance (no displacement), std::abs() is used.
 *
 * - revive()
 *      only sets active state to true and _used_move to 0 if applicable. But
 *      it's still in non-energetic mode because it shouldn't run as fast as
 *      it could at first, like in a sprint, runners cannot run fast like they
 *      started after stopping and catching their breaths. In other words,
 *      it's considered permanently inactive.
 *
 * - reset()
 *      only sets the GridFlea back to its initial location without compromising
 *      other properties such as _used_move.
 *
 *  A little complicated logic in checking whether the object should active or not
 *  when it has run out of the grid but within the accepting range. First, _active
 *  state will checked upon _hasJumpedOutside. If _hasJumpedOutside has not been
 *  marked as true, _active is still true. However, if the object has jumped outside,
 * _hasJumpedOutside will set to true, and in the next move() call, _active will
 * be set to false. That's why _active is checked before _hasJumpedOutside to
 * ensure gridflea is still allowed to move one more time when it has run out of
 * grid but still within the accepting range.
 */

#include "GridFlea.h"

GridFlea::GridFlea() : _size(5)
{
    _initialize_gridflea(0, 0);
    _initialize_max_move();
}

GridFlea::GridFlea(size_t x, size_t y, size_t size) : _size(std::max((int)size, 1))
{
    _initialize_gridflea(x, y);
    _initialize_max_move();
}

/**
 * Pre-conditions:
 *
 * The GridFlea must be active
 *
 * Post-conditions:
 *
 * The GridFlea might go to non-energetic, or inactive
 */
GridFlea GridFlea::operator+(int p)
{
    GridFlea tmp = *this;
    tmp.move(p);
    return tmp;
}

/**
 * Pre-conditions:
 *
 * The GridFlea must be active
 *
 * Post-conditions:
 *
 * The GridFlea might go to non-energetic, or inactive
 */
GridFlea& GridFlea::operator+=(int p)
{
    this->move(p);
    return *this;
}

/**
 * Pre-conditions:
 *
 * The GridFlea must be active
 *
 * Post-conditions:
 *
 * The GridFlea might go to non-energetic, or inactive
 */
GridFlea GridFlea::operator-(int p)
{
    GridFlea tmp = *this;
    tmp.move(-p);
    return tmp;
}

/**
 * Pre-conditions:
 *
 * The GridFlea must be active
 *
 * Post-conditions:
 *
 * The GridFlea might go to non-energetic, or inactive
 */
GridFlea& GridFlea::operator-=(int p)
{
    this->move(-p);
    return *this;
}

/**
 * Pre-conditions:
 *
 * The GridFlea must be active
 *
 * Post-conditions:
 *
 * The GridFlea might go to non-energetic, or inactive
 */
GridFlea& GridFlea::operator++()
{
    this->move(1);
    return *this;
}

/**
 * Pre-conditions:
 *
 * The GridFlea must be active
 *
 * Post-conditions:
 *
 * The GridFlea might go to non-energetic, or inactive
 */
GridFlea GridFlea::operator++(int)
{
    GridFlea tmp = *this;
    this->move(1);
    return tmp;
}

/**
 * Pre-conditions:
 *
 * The GridFlea must be active
 *
 * Post-conditions:
 *
 * The GridFlea might go to non-energetic, or inactive
 */
GridFlea& GridFlea::operator--()
{
    this->move(-1);
    return *this;
}

/**
 * Pre-conditions:
 *
 * The GridFlea must be active
 *
 * Post-conditions:
 *
 * The GridFlea might go to non-energetic, or inactive
 */
GridFlea GridFlea::operator--(int)
{
    GridFlea tmp = *this;
    this->move(-1);
    return tmp;
}

bool GridFlea::operator==(const GridFlea& other) const
{
    return this->value() == other.value();
}

bool GridFlea::operator<(const GridFlea& other) const
{
    return this->value() < other.value();
}

bool GridFlea::operator!=(const GridFlea& other) const
{
    return !(*this == other);
}

bool GridFlea::operator>(const GridFlea& other) const
{
    return other.value() < this->value();
}

bool GridFlea::operator<=(const GridFlea& other) const
{
    return !(*this > other);
}

bool GridFlea::operator>=(const GridFlea& other) const
{
    return !(*this < other);
}

/**
 * Pre-conditions:
 *
 * The GridFlea must be active
 *
 * Post-conditions:
 *
 * The GridFlea might go to non-energetic, or inactive
 */
void GridFlea::move(int p)
{
    if (!this->_active)
        return;

    bool is_even_step = p % 2 == 0;
    int actual_step = is_even_step ? p / 2 : p;
    int distance = std::abs(this->_energetic ? actual_step : 1);
    int displacement = p < 0 ? -distance : distance;

    if (is_even_step)
        this->_current_x += displacement;
    else
        this->_current_y += displacement;

    this->_used_move += static_cast<size_t>(distance);

    this->_energetic = this->_used_move <= this->_max_energetic_move;
    this->_active = !this->_hasJumpedOutside && (this->_used_move <= this->_max_active_move && !_is_outside_jump_exhaustive());
    this->_hasJumpedOutside = !this->_hasJumpedOutside || _is_outside_jump_exhaustive();
}

/**
 * Pre-conditions:
 *
 * The GridFlea must be active
 */
int GridFlea::value() const
{
    if (!this->_active)
        return -1;

    int change = std::abs(this->_initial_x - this->_current_x) + std::abs(this->_initial_y - this->_current_y);

    return static_cast<int>(this->_size * (REWARD - this->_used_move) * change);
}

/**
 * Pre-conditions:
 *
 * The GridFlea must be inactive and still in the grid
 *
 * Post-conditions:
 *
 * The GridFlea will be in active state
 */
void GridFlea::revive()
{
    if (this->_active || _is_outside_grid())
        return;

    this->_active = true;
    this->_used_move = 0;
    this->_hasJumpedOutside = false;
}

/**
 * Pre-conditions:
 *
 * The GridFlea must be  inactive and still in the grid
 */
void GridFlea::reset()
{
    if (this->_active || _is_outside_grid())
        return;

    this->_initialize_gridflea(this->_initial_x, this->_initial_y);
}

bool GridFlea::_is_outside_grid()
{
    return this->_current_x < 0 || this->_current_x > (int)XGRID || this->_current_y < 0 || this->_current_y > (int)YGRID;
}

bool GridFlea::_is_outside_jump_exhaustive()
{
    return this->_current_x < (int)(-EXTRAJUMP) || this->_current_x > (int)(XGRID + EXTRAJUMP) || this->_current_y < (int)(-EXTRAJUMP) || this->_current_y > (int)(YGRID + EXTRAJUMP);
}

void GridFlea::_initialize_gridflea(size_t x, size_t y)
{
    this->_initial_x = x <= XGRID ? x : XGRID;
    this->_initial_y = y <= YGRID ? y : YGRID;

    this->_current_x = this->_initial_x;
    this->_current_y = this->_initial_y;

    this->_active = true;
    this->_energetic = true;
    this->_hasJumpedOutside = false;
    this->_used_move = 0;
}

void GridFlea::_initialize_max_move()
{
    this->_max_active_move = (uint)((float)REWARD * (1.0 - ((float)(this->_size % REWARD) / REWARD)));
    this->_max_energetic_move = std::max((int)this->_max_active_move - 2, 0);
}
