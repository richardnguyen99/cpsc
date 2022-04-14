#include <cmath>
#include <algorithm>

#include "GridFlea.h"

GridFlea::GridFlea() : _size(5)
{
    _initialize_gridflea(0, 0);
}

GridFlea::GridFlea(size_t x, size_t y, size_t size) : _size(size)
{
    _initialize_gridflea(x, y);
}

GridFlea::GridFlea(const GridFlea &gridFlea) : _size(gridFlea._size)
{
    _initialize_gridflea(gridFlea._current_x, gridFlea._current_y);
}

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

    this->_energetic = this->_used_move <= ENERGETIC_MAX_MOVE;
    this->_active = this->_used_move <= ACTIVE_MAX_MOVE || _is_outside_grid();
}

int GridFlea::value()
{
    if (!this->_active)
        return -1;

    int change = std::abs(this->_initial_x - this->_current_x) + std::abs(this->_initial_y - this->_current_y);
    int reward = this->_used_move < REWARD ? (REWARD - this->_used_move) : 0;
    return static_cast<int>(this->_size * reward * change);
}

void GridFlea::revive()
{
    if (this->_active || _is_outside_grid())
        return;

    this->_active = true;
}

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

void GridFlea::_initialize_gridflea(size_t x, size_t y)
{
    this->_initial_x = x <= XGRID ? x : XGRID;
    this->_initial_y = y <= YGRID ? y : YGRID;

    this->_current_x = this->_initial_x;
    this->_current_y = this->_initial_y;

    this->_active = true;
    this->_energetic = true;
    this->_used_move = 0;
}
