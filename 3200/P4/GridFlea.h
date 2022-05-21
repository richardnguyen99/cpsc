/**
 * @file GridFlea.h
 * @version 0.2
 *
 * Class invariants - defined types and how to use GridFlea
 *
 * GridFlea is supposed to behave like a sprint. At the beginning of the race,
 * every GridFlea starts in energetic mode that allows them to run fast. After
 * some moves, a GridFlea goes to non-energetic mode, which can only move one
 * step at a time when move() is called no matter what step is passed in.
 *
 * After some more moves in non-energetic, it goes to inactive mode, which
 * cannot move at all when move() is called.
 *
 * Client might choose to revive() and reset() those inactive GridFleas with
 * condition to bring back those in active mode.
 *
 * A GridFlea when moves out of the boundary when still in the accepting range
 * will be marked once. A marked GridFlea will be still active and one following
 * move() will set the object to deactive no matter what. In that final move(),
 * if the gridflea goes back to boundaries, it can still be active via revive()
 * or reset().
 *
 * A GridFlea when moves out of the boundary completely becomes permanently
 * inactive, which cannot be brought back when revive() or reset() is called.
 *
 * The boundary is defined internally as well as the boundary checking.
 *
 * Each GridFlea has an associated value, called upon value() calculating how
 * efficiently it has moved. The more it moves, the less it returns the value.
 *
 * Interface invariants - how to use methods correctly
 *
 * - Constructor
 *  1. GridFlea()
 *      Default constructor constructs a GridFlea starting from (0,0). The
 *      default size is 5
 *
 *  2. GridFlea(size_t x, size_t y, size_t size)
 *      Parameter constructor constructs a GridFlea starting from (X, Y).
 *      GridFlea size will use SIZE
 *
 * - Assignment operator:
 *  1. Plus assignment +=(int p):
 *      Moves the GridFlea by P steps. If P is even, it will move P/2 steps
 *      on the X-axis, otherwise it will move P steps on the Y-axis. Moreover,
 *      GridFlea returns itself directly.
 *      Non-energetic GridFlea only moves one step on either direction and
 *      still accepts negative values.
 *      Inactive GridFleas will not move at all.
 *
 *  2. Minus assignment -=(int p):
 *      Moves the GridFlea by -P steps. If P is even, it will move P/2 steps
 *      on the X-axis, otherwise it will move P steps on the Y-axis. Moreover,
 *      GridFlea returns itself directly.
 *      Non-energetic GridFlea only moves one step on either direction and
 *      still accepts negative values.
 *      Inactive GridFleas will not move at all.
 *
 * - Addition and subtraction operator:
 *  1. operator+(int p):
 *      Moves the GridFlea by P steps. If P is even, it will move P/2 steps
 *      on the X-axis, otherwise it will move P steps on the Y-axis. Moreover,
 *      GridFlea returns a copy version of itself before moving.
 *      Non-energetic GridFlea only moves one step on either direction and
 *      still accepts negative values.
 *      Inactive GridFleas will not move at all.
 *
 *  2. operator-(int p):
 *      Moves the GridFlea by -P steps. If P is even, it will move P/2 steps
 *      on the X-axis, otherwise it will move P steps on the Y-axis. Moreover,
 *      GridFlea returns a copy version of itself before moving.
 *      Non-energetic GridFlea only moves one step on either direction and
 *      still accepts negative values.
 *      Inactive GridFleas will not move at all.
 *
 *  3. operator++(int): Post-increment
 *      Moves the GridFlea by 1 step. If P is even, it will move P/2 steps
 *      on the X-axis, otherwise it will move P steps on the Y-axis. Moreover,
 *      GridFlea returns a copy version of itself before moving.
 *      Non-energetic GridFlea only moves one step on either direction and
 *      still accepts negative values.
 *      Inactive GridFleas will not move at all.
 *
 *  4. operator--(int): Post-decrement
 *      Moves the GridFlea by -1 step. If P is even, it will move P/2 steps
 *      on the X-axis, otherwise it will move P steps on the Y-axis. Moreover,
 *      GridFlea returns a copy version of itself before moving.
 *      Non-energetic GridFlea only moves one step on either direction and
 *      still accepts negative values.
 *      Inactive GridFleas will not move at all.
 *
 *  5. operator++(): Pre-increment
 *      Moves the GridFlea by 1 steps. If P is even, it will move P/2 steps
 *      on the X-axis, otherwise it will move P steps on the Y-axis. Moreover,
 *      GridFlea returns itself directly.
 *      Non-energetic GridFlea only moves one step on either direction and
 *      still accepts negative values.
 *      Inactive GridFleas will not move at all.
 *
 *  6. operator--(): Pre-decrement
 *      Moves the GridFlea by -1 steps. If P is even, it will move P/2 steps
 *      on the X-axis, otherwise it will move P steps on the Y-axis. Moreover,
 *      GridFlea returns itself directly.
 *      Non-energetic GridFlea only moves one step on either direction and
 *      still accepts negative values.
 *      Inactive GridFleas will not move at all.

 * - Comparison operators:
 *      All overloaded comparison operators perform the associated logic.
 *      The compared object must be passed as const reference.
 *
 * - move(int p)
 *      Moves the GridFlea by P steps. If P is even, it will move P/2 steps
 *      on the X-axis, otherwise it will move P steps on the Y-axis. P also
 *      accepts negative values to move backward on the associating axis.
 *      (i.e. -4 will move 2 two steps backward on x-axis).
 *      Non-energetic GridFlea only moves one step on either direction and
 *      still accepts negative values.
 *      Inactive GridFleas will not move at all.
 *
 * - value()
 *      Computes the value based on the efficiency a GridFlea has moved so far.
 *      The bigger the size is, or the less the GridFlea has moved, the more
 *      it returns. Inactive gridfleas return -1.
 *
 * - revive()
 *      Attempts to bring back an inactive GridFlea to active state. If a gridflea
 *      has gone to inactive and outside the grid boundary, revive() will be not
 *      successful.
 *
 * - reset()
 *      Attempts to reset a GridFlea to initial location and states. If a gridflea
 *      has gone outside the grid boundary, reset() will be not successful.
 *
 */

#ifndef __GRIDFLEA_H
#define __GRIDFLEA_H 1

#include <cstddef>
#include <cmath>
#include <algorithm>

class GridFlea
{
private:
    using size_t = std::size_t;

    static constexpr size_t XGRID = 320;
    static constexpr size_t YGRID = 320;
    static constexpr size_t REWARD = 16;
    static constexpr size_t EXTRAJUMP = 8;

    int _initial_x;
    int _initial_y;
    int _current_x;
    int _current_y;

    size_t _size;
    size_t _used_move;
    size_t _max_energetic_move;
    size_t _max_active_move;

    bool _active;
    bool _energetic;
    bool _hasJumpedOutside;

    void _initialize_gridflea(size_t x, size_t y);
    bool _is_outside_grid();
    bool _is_outside_jump_exhaustive();
    void _initialize_max_move();

public:
    GridFlea();
    GridFlea(size_t x, size_t y, size_t size);

    GridFlea operator+(int p);
    GridFlea& operator+=(int p);
    GridFlea operator-(int p);
    GridFlea& operator-=(int p);
    GridFlea& operator++();
    GridFlea operator++(int);
    GridFlea& operator--();
    GridFlea operator--(int);

    bool operator==(const GridFlea& other) const;
    bool operator!=(const GridFlea& other) const;
    bool operator>(const GridFlea& other) const;
    bool operator<(const GridFlea& other) const;
    bool operator>=(const GridFlea& other) const;
    bool operator<=(const GridFlea& other) const;

    void move(int p);
    void revive();
    void reset();

    int value() const;
};

#endif // __GRIDFLEA_H
