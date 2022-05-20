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
 *  3. Plus assignment +=(int p):
 *     Trigger the entire GridFlea::move(p). Additionally, InFest keeps
 *     track of how many inactive GridFlea objects after this operator
 *     is called. If half of them is inactive, GridFlea::revive() might
 *     be called. Then, it returns the updated InFest

 *  4. Plus assignment +=(const GridFlea& gridFlea):
 *     Append the given gridFlea to the InFest and update its associated
 *     fields for other functions such as size(). Then, it returns the
 *     updated InFest
 *
 *  5. Minus assignment -=(int p):
 *     Trigger the entire GridFlea::move(-p). Additionally, InFest keeps
 *     track of how many inactive GridFlea objects after this operator
 *     is called. If half of them is inactive, GridFlea::revive() might
 *     be called. Then, it returns the updated InFest
 *
 * - Overloaded operator:
 *  1. operator+(int p):
 *     Create a new InFest and trigger the entire GridFlea::move(p).
 *     Additionally, InFest keeps track of how many inactive GridFlea
 *     objects after this operator is called. If half of them is
 *     inactive, GridFlea::revive() might be called. Then, it returns
 *     the newly-created-updated InFest.
 *
 *  2. operator-(int p):
 *     Create a new InFest and trigger the entire GridFlea::move(-p).
 *     Additionally, InFest keeps track of how many inactive GridFlea
 *     objects after this operator is called. If half of them is
 *     inactive, GridFlea::revive() might be called. Then, it returns
 *     the newly-created-updated InFest.
 *
 *  3. operator++(int): Post-increment
 *     Create a new InFest and trigger the entire GridFlea::move(1).
 *     Additionally, InFest keeps track of how many inactive GridFlea
 *     objects after this operator is called. If half of them is
 *     inactive, GridFlea::revive() might be called. Then, it returns
 *     the newly-created-updated InFest.
 *
 *  4. operator--(int): Post-decrement
 *     Create a new InFest and trigger the entire GridFlea::move(-1).
 *     Additionally, InFest keeps track of how many inactive GridFlea
 *     objects after this operator is called. If half of them is
 *     inactive, GridFlea::revive() might be called. Then, it returns
 *     the newly-created-updated InFest.
 *
 *  5. operator++(): Pre-increment
 *     Trigger the entire GridFlea::move(1). Additionally, InFest keeps
 *     track of how many inactive GridFlea objects after this operator
 *     is called. If half of them is inactive, GridFlea::revive() might
 *     be called. Then, it returns the updated InFest.
 *
 *  6. operator--(): Pre-decrement
 *     Trigger the entire GridFlea::move(-1). Additionally, InFest keeps
 *     track of how many inactive GridFlea objects after this operator
 *     is called. If half of them is inactive, GridFlea::revive() might
 *     be called. Then, it returns the updated InFest.
 *
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
 *
 * - size(): returns the number of GridFlea objects in InFest
 *
 * - value(): returns the accumulative value from each GridFlea object in InFest
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

    InFest operator+(int p);
    InFest& operator+=(int p);
    InFest operator-(int p);
    InFest& operator-=(int p);
    InFest& operator++();
    InFest operator++(int);
    InFest& operator--();
    InFest operator--(int);


    InFest &operator=(const InFest &inFest);
    InFest &operator=(InFest &&inFest);
    InFest operator+(const GridFlea& gridFlea);
    InFest& operator+=(const GridFlea& gridFlea);

    bool operator==(const InFest& other) const;
    bool operator!=(const InFest& other) const;
    bool operator>(const InFest& other) const;
    bool operator<(const InFest& other) const;
    bool operator>=(const InFest& other) const;
    bool operator<=(const InFest& other) const;

    void move(int p);

    int min() const;
    int value() const;
    int size() const;
    int max() const;
};

#endif // __INFREST_H
