#ifndef __GRIDFLEA_H
#define __GRIDFLEA_H 1

#include <cstddef>

class GridFlea
{
private:
    using size_t = std::size_t;

    static constexpr size_t XGRID = 320;
    static constexpr size_t YGRID = 320;
    static constexpr size_t REWARD = 16;
    static constexpr size_t ENERGETIC_MAX_MOVE = 6;
    static constexpr size_t ACTIVE_MAX_MOVE = 12;

    int _initial_x;
    int _initial_y;
    int _current_x;
    int _current_y;

    size_t _size;
    size_t _used_move;

    bool _active;
    bool _energetic;

    void _initialize_gridflea(size_t x, size_t y);
    bool _is_outside_grid();

public:
    GridFlea();
    GridFlea(size_t x, size_t y, size_t size);
    GridFlea(const GridFlea &gridFlea);

    void move(int p);
    int value();
    void revive();
    void reset();
};

#endif // __GRIDFLEA_H
