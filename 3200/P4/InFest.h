/**
 * @file InFest.h
 *
 * Class invariants - defined types and how to use InFest
 *
 * InFest encapsulates an array of GridFlea objects, and
 * responds consistently with the actions as well as the
 * states of individual GridFlea objects. All actions in
 * InFest will trigger the according action in each GridFlea
 * objects.
 *
 * The states of the InFest is handled automatically and
 * internally, being restored and changed state if needed.
 *
 * InFest will start with initial state GridFlea objects,
 * which means each is active and energetic at first.
 *
 * move() and value(), as mentioned above, will the trigger
 * move() and value() in every GridFlea, because you know,
 * it's infested.
 *
 * Because GridFlea is a dangerous species, whenever less than
 * half of them is inactive and still inside the grid, InFest
 * can restore all of them to the active state again.
 *
 * In order to do that, InFest keeps track of how many GridFlea
 * goes into iactive state after move() was called.
 *
 * Interface invariants - How to use methods correctly
 *
 * - Constructor:
 *  1. constructs an InFest with client's specified specs like.
 *  2. constructs an InFest by deep-copying an existing InFest
 *  3. constructs an InFest by tranfering ownership from an
 *     existing InFest to the new one
 *
 * - Destructor: destructs the InFest. The used array is
 *               deallocated and its memory is returned to heap.
 *
 * - Assignment overator:
 *  1. copy assignment - Replace the contents with a copy of the
 *                       contents of the other InFest
 *  2. Move assignment - Replace the contens with the contents of
 *                       the other InFest using move semantics
 *                       (i.e. the data in other is transfered
 *                       to this InFest).
 *
 * - move(int p): triggers the entire GridFlea::move(p). Additionally,
 *                InFest also keeps tracks of how many inactive objects
 *                after move() is called. If half of them is inactive,
 *                GridFlea::revive() might be called
 *
 * - min(): returns the minimum value in the GridFlea array based on
 *          GridFlea::value(), or -2 if the InFest is invalid
 *
 * - max(): returns the maximum value in the GridFlea array based on
 *          GridFlea::value(), or -2 if the InFest is invalid

 * - size(): return the number of GridFlea objects in InFest
 */

#ifndef __INFEST_H
#define __INFEST_H 1

#include <cstddef>
#include <cstdlib>
#include <memory>

#include "GridFlea.h"

class InFest
{
private:
    using size_type = std::size_t;

    size_type _num;
    size_type _inactive_num;
    GridFlea *_gridfleas;

public:
    InFest();
    InFest(size_type x, size_t y, size_t gridflea_size, size_t gridflea_num);
    InFest(const InFest &inFest);
    InFest(InFest &&inFest);
    ~InFest();

    InFest &operator=(const InFest &inFest);
    InFest &operator=(InFest &&inFest);

    void move(int p);
    size_type size();
    int min();
    int max();
};

#endif // __INFREST_H
