#include <random>
#include <iostream>
#include <memory>
#include <vector>
#include <thread>
#include <chrono>
#include <mutex>

#include "InFest.h"

using std::cout;
using std::endl;
using InFestPtr = std::unique_ptr<InFest>;
using InFestCollection = std::vector<InFestPtr>;
using GridFleaPtr = std::unique_ptr<GridFlea>;
using GridFleaCollection = std::vector<GridFleaPtr>;

#define SAMPLE 10

std::string format_msg(bool status)
{
    return status == true ? "DONE" : "FAILED";
}

void initialialize_args(std::size_t initialialize_args[][10])
{
    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> grid_range(0, 320);
    std::uniform_int_distribution<std::size_t> size_range(0, 15);
    std::uniform_int_distribution<std::size_t> num_range(1, 10);

    for (int i = 0; i < SAMPLE; i++)
    {
        initialialize_args[0][i] = grid_range(generator);
        initialialize_args[1][i] = grid_range(generator);
        initialialize_args[2][i] = size_range(generator);
        initialialize_args[3][i] = num_range(generator);
    }
}

GridFlea* find_nearest_active_gridflea(GridFleaCollection& gridflea_ptrs)
{
    for (GridFleaPtr& ptr : gridflea_ptrs)
    {
        if (ptr->value() != -1)
            return ptr.get();
    }

    return nullptr;
}

// void test_initialize(std::vector<std::unique_ptr<InFest>>& infest_ptrs, std::size_t initial_args[][10])
void test_initialize(InFestCollection &infest_ptrs, std::size_t initial_args[][10])
{
    cout << "\nTesting InFest initialization" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        std::size_t x = initial_args[0][i];
        std::size_t y = initial_args[1][i];
        std::size_t size = initial_args[2][i];
        std::size_t num = initial_args[3][i];

        cout << "\tInitialize InFest(" << x << ", " << y << ", " << size << ", " << num << ")" << endl;
        infest_ptrs[i] = std::make_unique<InFest>(x, y, size, num);
    }

    cout << "Tested InFest initialization\t\t\t" << format_msg(status) << endl;
}

void test_min(InFestCollection &infest_ptrs)
{
    cout << "\nTesting InFest::min()" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        cout << "\tInFests[" << i << "].min() = " << infest_ptrs[i]->min() << endl;
    }

    cout << "Tested InFest::min()\t\t\t\t" << format_msg(status) << endl;
}

void test_max(InFestCollection &infest_ptrs)
{
    cout << "\nTesting InFest::max()" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        cout << "\tInFests[" << i << "].max() = " << infest_ptrs[i]->max() << endl;
    }

    cout << "Tested InFest::max()\t\t\t\t" << format_msg(status) << endl;
}

void test_move(InFestCollection &infest_ptrs)
{
    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> move_range(10, 100);

    cout << "\nTesting InFest::move()" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        std::size_t step = move_range(generator);
        cout << "\tInFests[" << i << "].move(" << step << ")" << endl;
        infest_ptrs[i]->move(step);
    }

    cout << "Tested InFest::move()\t\t\t\t" << format_msg(status) << endl;
}

void test_copy(const InFestCollection &src, InFestCollection &dest, std::size_t initial_args[][10])
{
    cout << "\nTesting InFest copy constructor" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        std::size_t x = initial_args[0][i];
        std::size_t y = initial_args[1][i];
        std::size_t size = initial_args[2][i];
        std::size_t num = initial_args[3][i];

        cout << "\tCopy InFest(" << x << ", " << y << ", " << size << ", " << num << ")" << endl;
        dest[i] = std::make_unique<InFest>(*(src[i]));
    }

    cout << "Tested InFest copy constructor\t\t\t" << format_msg(status) << endl;
}

void test_copy2(const InFestCollection &src, InFestCollection &dest)
{
    cout << "\nTesting InFest copy constructor" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
        dest[i] = std::make_unique<InFest>(*(src[i]));

    cout << "Tested InFest copy constructor\t\t\t" << format_msg(status) << endl;
}

void test_move_semantic(const InFestCollection &src, InFestCollection &dest, std::size_t initial_args[][10])
{
    cout << "\nTesting InFest move constructor" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        std::size_t x = initial_args[0][i];
        std::size_t y = initial_args[1][i];
        std::size_t size = initial_args[2][i];
        std::size_t num = initial_args[3][i];

        cout << "\tMove InFest(" << x << ", " << y << ", " << size << ", " << num << ")" << endl;
        dest[i] = std::make_unique<InFest>(std::move(*(src[i])));
    }

    cout << "Tested InFest move constructor\t\t\t" << format_msg(status) << endl;
}

void test_copy_assignment(const InFestCollection &src, InFestCollection &dest)
{
    cout << "\nTesting InFest::operator=" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        dest[i].reset(nullptr);
        dest[i] = std::make_unique<InFest>(*(src[i]));
    }

    cout << "Tested InFest::operator=\t\t\t\t" << format_msg(status) << endl;
}

void test_move_copy_assignment(InFestCollection &src, InFestCollection &dest)
{
    cout << "\nTesting InFest::operator=(&&)" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        dest[i].reset(nullptr);
        dest[i] = std::make_unique<InFest>(std::move(*(src[i])));
    }

    cout << "Tested InFest::operator=(&&)\t\t\t\t" << format_msg(status) << endl;
}

void test_shared_ptr()
{
    cout << "\nTesting shared_ptr\n";

    bool status = true;
    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> grid_range(0, 320);
    std::uniform_int_distribution<std::size_t> size_range(0, 15);
    std::uniform_int_distribution<std::size_t> num_range(1, 10);

    std::size_t x = grid_range(generator);
    std::size_t y = grid_range(generator);
    std::size_t size = size_range(generator);
    std::size_t num = num_range(generator);

    std::shared_ptr<InFest> shared_infest = std::make_shared<InFest>(x, y, size, num);

    cout << "Created a shared InFest pointer\n"
         << "\tActual object address: " << shared_infest.get() << "\n"
         << "\tReference count: " << shared_infest.use_count() << "\n";

    {
        std::vector<std::shared_ptr<InFest>> shares(5);
        for (std::size_t i = 0; i < shares.size(); i++)
        {
            shares[i] = shared_infest;
            cout << "- Shared InFest ID " << (i + 1) << ":\n"
                 << "\tActual object address: " << shares[i].get() << "\n"
                 << "\tReference count: " << shares[i].use_count() << "\n";
        }

        cout << "\n- Exited sharing section\n";
    }

    cout << "\nAfter shared: \n"
         << "\tActual object address: " << shared_infest.get() << "\n"
         << "\tReference count: " << shared_infest.use_count() << "\n";

    cout << "Tested shared InFest pointer\t\t\t\t" << format_msg(status) << endl;
}

void test_shared_ptr_with_reset()
{
    cout << "\nTesting shared_ptr with reset()\n";

    bool status = true;
    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> grid_range(0, 320);
    std::uniform_int_distribution<std::size_t> size_range(0, 15);
    std::uniform_int_distribution<std::size_t> num_range(1, 10);

    std::size_t x = grid_range(generator);
    std::size_t y = grid_range(generator);
    std::size_t size = size_range(generator);
    std::size_t num = num_range(generator);

    std::shared_ptr<InFest> shared_infest = std::make_shared<InFest>(x, y, size, num);

    cout << "Created a shared InFest pointer\n"
         << "\tActual object address: " << shared_infest.get() << "\n"
         << "\tReference count: " << shared_infest.use_count() << "\n";

    {
        std::vector<std::shared_ptr<InFest>> shares(5);
        for (std::size_t i = 0; i < shares.size(); i++)
        {
            shares[i] = shared_infest;
            cout << "- Shared InFest ID " << (i + 1) << ":\n"
                 << "\tActual object address: " << shares[i].get() << "\n"
                 << "\tReference count: " << shares[i].use_count() << "\n";
        }

        cout << "\n- Reset the original shared pointer\n\n";
        shared_infest.reset();

        for (std::size_t i = 0; i < shares.size(); i++)
        {
            cout << "- Shared InFest ID " << (i + 1) << ":\n"
                 << "\tActual object address: " << shares[i].get() << "\n"
                 << "\tReference count: " << shares[i].use_count() << "\n";
            shares[i].reset();
        }

        cout << "\n- Exited sharing section\n";
    }

    cout << "\nAfter shared and reset: \n"
         << "\tActual object address: " << shared_infest.get() << "\n"
         << "\tReference count: " << shared_infest.use_count() << "\n";

    cout << "Tested shared InFest pointer\t\t\t\t" << format_msg(status) << endl;
}

void test_initialize_gridfleas(GridFleaCollection &gridflea_ptrs, std::size_t initial_args[][10])
{
    cout << "\nTest initializing of grid fleas\n";

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        std::size_t x = initial_args[0][i];
        std::size_t y = initial_args[1][i];
        std::size_t size = initial_args[2][i];

        cout << "\tInitialize GridFlea(" << x << ", " << y << ", " << size << ")" << endl;
        gridflea_ptrs[i] = std::make_unique<GridFlea>(x, y, size);
    }

    cout << "Tested initializing of grid fleas\t\t\t" << format_msg(status) << endl;
}

void test_gridfleas_ltoperator(GridFleaCollection &gridflea_ptrs, const GridFlea &rhs)
{
    cout << "\nTest GridFlea::operator<()\n";

    int test_value = rhs.value();
    cout << "\nTest GridFlea Object with value: " << test_value << "\n\n";

    bool status = true;
    int i = 0;
    for (GridFleaPtr &gridflea_ptr : gridflea_ptrs)
    {
        std::string result = *gridflea_ptr < rhs ? "true" : "false";
        int actual_value = gridflea_ptr->value();

        cout
            << "\tGridFlea[" << i << "] (value = " << actual_value << ") < test GridFlea?: " << result << "\n";
        i++;
    }

    cout << "\nTested GridFlea::operator<()\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_eqoperator(GridFleaCollection &gridflea_ptrs, const GridFlea &rhs)
{
    cout << "\nTest GridFlea::operator==()\n";

    int test_value = rhs.value();
    cout << "\nTest GridFlea Object with value: " << test_value << "\n\n";

    bool status = true;
    int i = 0;
    for (GridFleaPtr &gridflea_ptr : gridflea_ptrs)
    {
        std::string result = *gridflea_ptr == rhs ? "true" : "false";
        int actual_value = gridflea_ptr->value();

        cout
            << "\tGridFlea[" << i << "] (value = " << actual_value << ") == test GridFlea?: " << result << "\n";
        i++;
    }

    cout << "\nTested GridFlea::operator==()\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_neoperator(GridFleaCollection &gridflea_ptrs, const GridFlea &rhs)
{
    cout << "\nTest GridFlea::operator!=()\n";

    int test_value = rhs.value();
    cout << "\nTest GridFlea Object with value: " << test_value << "\n\n";

    bool status = true;
    int i = 0;
    for (GridFleaPtr &gridflea_ptr : gridflea_ptrs)
    {
        std::string result = *gridflea_ptr != rhs ? "true" : "false";
        int actual_value = gridflea_ptr->value();

        cout
            << "\tGridFlea[" << i << "] (value = " << actual_value << ") != test GridFlea?: " << result << "\n";
        i++;
    }

    cout << "\nTested GridFlea::operator!=()\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_gtoperator(GridFleaCollection &gridflea_ptrs, const GridFlea &rhs)
{
    cout << "\nTest GridFlea::operator>()\n";

    int test_value = rhs.value();
    cout << "\nTest GridFlea Object with value: " << test_value << "\n\n";

    bool status = true;
    int i = 0;
    for (GridFleaPtr &gridflea_ptr : gridflea_ptrs)
    {
        std::string result = *gridflea_ptr > rhs ? "true" : "false";
        int actual_value = gridflea_ptr->value();

        cout
            << "\tGridFlea[" << i << "] (value = " << actual_value << ") > test GridFlea?: " << result << "\n";
        i++;
    }

    cout << "\nTested GridFlea::operator>()\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_geoperator(GridFleaCollection &gridflea_ptrs, const GridFlea &rhs)
{
    cout << "\nTest GridFlea::operator>=()\n";

    int test_value = rhs.value();
    cout << "\nTest GridFlea Object with value: " << test_value << "\n\n";

    bool status = true;
    int i = 0;
    for (GridFleaPtr &gridflea_ptr : gridflea_ptrs)
    {
        std::string result = *gridflea_ptr >= rhs ? "true" : "false";
        int actual_value = gridflea_ptr->value();

        cout
            << "\tGridFlea[" << i << "] (value = " << actual_value << ") >= test GridFlea?: " << result << "\n";
        i++;
    }

    cout << "\nTested GridFlea::operator>=()\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_leoperator(GridFleaCollection &gridflea_ptrs, const GridFlea &rhs)
{
    cout << "\nTest GridFlea::operator<=()\n";

    int test_value = rhs.value();
    cout << "\nTest GridFlea Object with value: " << test_value << "\n\n";

    bool status = true;
    int i = 0;
    for (GridFleaPtr &gridflea_ptr : gridflea_ptrs)
    {
        std::string result = *gridflea_ptr <= rhs ? "true" : "false";
        int actual_value = gridflea_ptr->value();

        cout
            << "\tGridFlea[" << i << "] (value = " << actual_value << ") <= test GridFlea?: " << result << "\n";
        i++;
    }

    cout << "\nTested GridFlea::operator<=()\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_nonassigned_plusop(GridFleaCollection &gridflea_ptrs)
{
    cout << "\nTest GridFlea::operator+(int p)\n";

    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> step_range(1, 5);

    std::size_t step = step_range(generator);

    cout << "\nMove GridFlea objects step: " << step << "\n";

    bool status = true;
    for (std::size_t i = 0; i < gridflea_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] before moving value: " << old_value << "\n";

        (*gridflea_ptrs[i]) + step;

        int new_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] after moving value: " << new_value << "\n";
    }


    cout << "\nTestd GridFlea::operator+(int p)\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_nonassigned_minusop(GridFleaCollection &gridflea_ptrs)
{
    cout << "\nTest GridFlea::operator-(int p)\n";

    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> step_range(1, 5);

    std::size_t step = step_range(generator);

    cout << "\nMove GridFlea objects step: " << step << "\n";

    bool status = true;
    for (std::size_t i = 0; i < gridflea_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] before moving value: " << old_value << "\n";

        (*gridflea_ptrs[i]) - step;

        int new_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] after moving value: " << new_value << "\n";
    }


    cout << "\nTestd GridFlea::operator-(int p)\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_assigned_minusop(GridFleaCollection &gridflea_ptrs)
{
    cout << "\nTest GridFlea::operator-=(int p)\n";

    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> step_range(1, 5);

    std::size_t step = step_range(generator);

    cout << "\nMove GridFlea objects step: " << step << "\n";

    bool status = true;
    for (std::size_t i = 0; i < gridflea_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] before moving value: " << old_value << "\n";

        (*gridflea_ptrs[i]) -= step;

        int new_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] after moving value: " << new_value << "\n";
    }


    cout << "\nTestd GridFlea::operator-=(int p)\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_assigned_plusop(GridFleaCollection &gridflea_ptrs)
{
    cout << "\nTest GridFlea::operator+=(int p)\n";

    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> step_range(1, 5);

    std::size_t step = step_range(generator);

    cout << "\nMove GridFlea objects step: " << step << "\n";

    bool status = true;
    for (std::size_t i = 0; i < gridflea_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] before moving value: " << old_value << "\n";

        (*gridflea_ptrs[i]) += step;

        int new_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] after moving value: " << new_value << "\n";
    }


    cout << "\nTestd GridFlea::operator+=(int p)\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_postincrement(GridFleaCollection &gridflea_ptrs)
{
    cout << "\nTest GridFlea::operator++(int p)\n";

    bool status = true;
    for (std::size_t i = 0; i < gridflea_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] before post-increment: " << old_value << "\n";

        GridFlea tmp = (*gridflea_ptrs[i])++;

        int new_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] after post-increment: " << new_value << "\n";
        cout << "New temp gridflea after post-increment: " << tmp.value() << "\n";
    }


    cout << "\nTestd GridFlea::operator++(int p)\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_preincrement(GridFleaCollection &gridflea_ptrs)
{
    cout << "\nTest GridFlea::operator++()\n";

    bool status = true;
    for (std::size_t i = 0; i < gridflea_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] before pre-increment: " << old_value << "\n";

        GridFlea tmp = ++(*gridflea_ptrs[i]);

        int new_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] after pre-increment: " << new_value << "\n";
        cout << "New temp gridflea after pre-increment: " << tmp.value() << "\n";
    }


    cout << "\nTestd GridFlea::operator++()\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_postdecrement(GridFleaCollection &gridflea_ptrs)
{
    cout << "\nTest GridFlea::operator--(int p)\n";

    bool status = true;
    for (std::size_t i = 0; i < gridflea_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] before post-decrement: " << old_value << "\n";

        GridFlea tmp = (*gridflea_ptrs[i])--;

        int new_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] after post-decrement: " << new_value << "\n";
        cout << "New temp gridflea after post-decrement: " << tmp.value() << "\n";
    }


    cout << "\nTestd GridFlea::operator--(int p)\t\t\t" << format_msg(status) << "\n";
}

void test_gridfleas_predecrement(GridFleaCollection &gridflea_ptrs)
{
    cout << "\nTest GridFlea::operator--()\n";

    bool status = true;
    for (std::size_t i = 0; i < gridflea_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] before pre-decrement: " << old_value << "\n";

        GridFlea tmp = --(*gridflea_ptrs[i]);

        int new_value = gridflea_ptrs[i]->value();
        cout << "GridFlea["<< i <<"] after pre-decrement: " << new_value << "\n";
        cout << "New temp gridflea after pre-decrement: " << tmp.value() << "\n";
    }


    cout << "\nTestd GridFlea::operator--()\t\t\t" << format_msg(status) << "\n";
}

void test_infests_ltoperator(InFestCollection &infest_ptrs, const InFest &rhs)
{
    cout << "\nTest InFest::operator<()\n";

    int test_value = rhs.value();
    cout << "\nTest InFest Object with value: " << test_value << "\n\n";

    bool status = true;
    int i = 0;
    for (InFestPtr &infest_ptr : infest_ptrs)
    {
        std::string result = *infest_ptr < rhs ? "true" : "false";
        int actual_value = infest_ptr->value();

        cout
            << "\tInFest[" << i << "] (value = " << actual_value << ") < test InFest?: " << result << "\n";
        i++;
    }

    cout << "\nTested InFest::operator<()\t\t\t" << format_msg(status) << "\n";
}

void test_infests_leoperator(InFestCollection &infest_ptrs, const InFest &rhs)
{
    cout << "\nTest InFest::operator<=()\n";

    int test_value = rhs.value();
    cout << "\nTest InFest Object with value: " << test_value << "\n\n";

    bool status = true;
    int i = 0;
    for (InFestPtr &infest_ptr : infest_ptrs)
    {
        std::string result = *infest_ptr <= rhs ? "true" : "false";
        int actual_value = infest_ptr->value();

        cout
            << "\tInFest[" << i << "] (value = " << actual_value << ") <= test InFest?: " << result << "\n";
        i++;
    }

    cout << "\nTested InFest::operator<=()\t\t\t" << format_msg(status) << "\n";
}
void test_infests_gtoperator(InFestCollection &infest_ptrs, const InFest &rhs)
{
    cout << "\nTest InFest::operator>()\n";

    int test_value = rhs.value();
    cout << "\nTest InFest Object with value: " << test_value << "\n\n";

    bool status = true;
    int i = 0;
    for (InFestPtr &infest_ptr : infest_ptrs)
    {
        std::string result = *infest_ptr > rhs ? "true" : "false";
        int actual_value = infest_ptr->value();

        cout
            << "\tInFest[" << i << "] (value = " << actual_value << ") > test InFest?: " << result << "\n";
        i++;
    }

    cout << "\nTested InFest::operator>()\t\t\t" << format_msg(status) << "\n";
}

void test_infests_geoperator(InFestCollection &infest_ptrs, const InFest &rhs)
{
    cout << "\nTest InFest::operator>=()\n";

    int test_value = rhs.value();
    cout << "\nTest InFest Object with value: " << test_value << "\n\n";

    bool status = true;
    int i = 0;
    for (InFestPtr &infest_ptr : infest_ptrs)
    {
        std::string result = *infest_ptr >= rhs ? "true" : "false";
        int actual_value = infest_ptr->value();

        cout
            << "\tInFest[" << i << "] (value = " << actual_value << ") >= test InFest?: " << result << "\n";
        i++;
    }

    cout << "\nTested InFest::operator>=()\t\t\t" << format_msg(status) << "\n";
}

void test_infests_eqoperator(InFestCollection &infest_ptrs, const InFest &rhs)
{
    cout << "\nTest InFest::operator==()\n";

    int test_value = rhs.value();
    cout << "\nTest InFest Object with value: " << test_value << "\n\n";

    bool status = true;
    int i = 0;
    for (InFestPtr &infest_ptr : infest_ptrs)
    {
        std::string result = *infest_ptr == rhs ? "true" : "false";
        int actual_value = infest_ptr->value();

        cout
            << "\tInFest[" << i << "] (value = " << actual_value << ") == test InFest?: " << result << "\n";
        i++;
    }

    cout << "\nTested InFest::operator==()\t\t\t" << format_msg(status) << "\n";
}

void test_infests_neoperator(InFestCollection &infest_ptrs, const InFest &rhs)
{
    cout << "\nTest InFest::operator!=()\n";

    int test_value = rhs.value();
    cout << "\nTest InFest Object with value: " << test_value << "\n\n";

    bool status = true;
    int i = 0;
    for (InFestPtr &infest_ptr : infest_ptrs)
    {
        std::string result = *infest_ptr != rhs ? "true" : "false";
        int actual_value = infest_ptr->value();

        cout
            << "\tInFest[" << i << "] (value = " << actual_value << ") != test InFest?: " << result << "\n";
        i++;
    }

    cout << "\nTested InFest::operator!=()\t\t\t" << format_msg(status) << "\n";
}

void test_infests_nonassigned_plusop(InFestCollection &infest_ptrs)
{
    cout << "\nTest InFest::operator+(int p)\n";

    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> step_range(1, 3);

    std::size_t step = step_range(generator);

    cout << "\nMove infest objects step: " << step << "\n";

    bool status = true;
    for (std::size_t i = 0; i < infest_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = infest_ptrs[i]->value();
        cout << "infest["<< i <<"] before moving value: " << old_value << "\n";

        (*infest_ptrs[i]) + step;

        int new_value = infest_ptrs[i]->value();
        cout << "infest["<< i <<"] after moving value: " << new_value << "\n";
    }


    cout << "\nTestd InFest::operator+(int p)\t\t\t" << format_msg(status) << "\n";
}

void test_infests_nonassigned_minusop(InFestCollection &infest_ptrs)
{
    cout << "\nTest InFest::operator-(int p)\n";

    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> step_range(1, 3);

    std::size_t step = step_range(generator);

    cout << "\nMove infest objects step: " << step << "\n";

    bool status = true;
    for (std::size_t i = 0; i < infest_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = infest_ptrs[i]->value();
        cout << "infest["<< i <<"] before moving value: " << old_value << "\n";

        (*infest_ptrs[i]) - step;

        int new_value = infest_ptrs[i]->value();
        cout << "infest["<< i <<"] after moving value: " << new_value << "\n";
    }


    cout << "\nTestd InFest::operator-(int p)\t\t\t" << format_msg(status) << "\n";
}

void test_infests_assigned_minusop(InFestCollection &infest_ptrs)
{
    cout << "\nTest InFest::operator-=(int p)\n";

    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> step_range(1, 3);

    std::size_t step = step_range(generator);

    cout << "\nMove infest objects step: " << step << "\n";

    bool status = true;
    for (std::size_t i = 0; i < infest_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = infest_ptrs[i]->value();
        cout << "infest["<< i <<"] before moving value: " << old_value << "\n";

        (*infest_ptrs[i]) -= step;

        int new_value = infest_ptrs[i]->value();
        cout << "infest["<< i <<"] after moving value: " << new_value << "\n";
    }


    cout << "\nTestd InFest::operator-=(int p)\t\t\t" << format_msg(status) << "\n";
}

void test_infests_assigned_plusop(InFestCollection &infest_ptrs)
{
    cout << "\nTest InFest::operator+=(int p)\n";

    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> step_range(1, 3);

    std::size_t step = step_range(generator);

    cout << "\nMove infest objects step: " << step << "\n";

    bool status = true;
    for (std::size_t i = 0; i < infest_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = infest_ptrs[i]->value();
        cout << "infest["<< i <<"] before moving value: " << old_value << "\n";

        (*infest_ptrs[i]) += step;

        int new_value = infest_ptrs[i]->value();
        cout << "infest["<< i <<"] after moving value: " << new_value << "\n";
    }


    cout << "\nTestd InFest::operator+=(int p)\t\t\t" << format_msg(status) << "\n";
}

void test_infests_postincrement(InFestCollection &infest_ptrs)
{
    cout << "\nTest InFest::operator++(int p)\n";

    bool status = true;
    for (std::size_t i = 0; i < infest_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = infest_ptrs[i]->value();
        cout << "InFest["<< i <<"] before post-increment: " << old_value << "\n";

        InFest tmp = (*infest_ptrs[i])++;

        int new_value = infest_ptrs[i]->value();
        cout << "InFest["<< i <<"] after post-increment: " << new_value << "\n";
        cout << "New temp infest after post-increment: " << tmp.value() << "\n";
    }


    cout << "\nTestd InFest::operator++(int p)\t\t\t" << format_msg(status) << "\n";
}

void test_infests_preincrement(InFestCollection &infest_ptrs)
{
    cout << "\nTest InFest::operator++()\n";

    bool status = true;
    for (std::size_t i = 0; i < infest_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = infest_ptrs[i]->value();
        cout << "InFest["<< i <<"] before pre-increment: " << old_value << "\n";

        InFest tmp = ++(*infest_ptrs[i]);

        int new_value = infest_ptrs[i]->value();
        cout << "InFest["<< i <<"] after pre-increment: " << new_value << "\n";
        cout << "New temp infest after pre-increment: " << tmp.value() << "\n";
    }

    cout << "\nTestd InFest::operator++()\t\t\t" << format_msg(status) << "\n";
}

void test_infests_postdecrement(InFestCollection &infest_ptrs)
{
    cout << "\nTest InFest::operator--(int p)\n";

    bool status = true;
    for (std::size_t i = 0; i < infest_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = infest_ptrs[i]->value();
        cout << "InFest["<< i <<"] before post-decrement: " << old_value << "\n";

        InFest tmp = (*infest_ptrs[i])--;

        int new_value = infest_ptrs[i]->value();
        cout << "InFest["<< i <<"] after post-decrement: " << new_value << "\n";
        cout << "New temp infest after post-decrement: " << tmp.value() << "\n";
    }


    cout << "\nTestd InFest::operator--(int p)\t\t\t" << format_msg(status) << "\n";
}

void test_infests_predecrement(InFestCollection &infest_ptrs)
{
    cout << "\nTest InFest::operator--()\n";

    bool status = true;
    for (std::size_t i = 0; i < infest_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = infest_ptrs[i]->value();
        cout << "InFest["<< i <<"] before pre-decrement: " << old_value << "\n";

        InFest tmp = --(*infest_ptrs[i]);

        int new_value = infest_ptrs[i]->value();
        cout << "InFest["<< i <<"] after pre-decrement: " << new_value << "\n";
        cout << "New temp infest after pre-decrement: " << tmp.value() << "\n";
    }

    cout << "\nTestd InFest::operator--()\t\t\t" << format_msg(status) << "\n";
}

void test_infests_nonassign_add_gridflea(InFestCollection& infest_ptrs, GridFleaCollection& gridflea_ptrs)
{
    cout << "\nTest InFest::operator+(const GridFlea& gridFlea)\n";

    GridFlea *activeGridFlea = find_nearest_active_gridflea(gridflea_ptrs);

    if (activeGridFlea == nullptr)
    {
        cout << "Re-run to perform test_infests_nonassign_add_gridflea() since there is no active gridflea object\n";
        cout << "\nTested InFest::operator+(const GridFlea& gridFlea)\t\t\t" << format_msg(false) << "\n";
        return;
    }

    cout << "\nAdd GridFlea with value: " << activeGridFlea->value() << "\n";

    for (std::size_t i = 0; i < infest_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = infest_ptrs[i]->value();
        int old_size = infest_ptrs[i]->size();
        cout << "InFest["<< i <<"] value before adding GridFlea: " << old_value << "\n";
        cout << "InFest["<< i <<"] size before adding GridFlea: " << old_size << "\n";

        InFest tmp = (*infest_ptrs[i]) + *activeGridFlea;

        int new_value = infest_ptrs[i]->value();
        int new_size = infest_ptrs[i]->size();
        cout << "\nInFest["<< i <<"] value after adding GridFlea: " << new_value << "\n";
        cout << "InFest["<< i <<"] size after adding GridFlea: " << new_size << "\n";
        cout << "New temp infest after adding GridFlea: " << tmp.value() << "\n";
        cout << "New temp infest after adding GridFlea: " << tmp.size() << "\n";
    }

    cout << "\nTested InFest::operator+(const GridFlea& gridFlea)\t" << format_msg(true) << "\n";
}

void test_infests_assign_add_gridflea(InFestCollection& infest_ptrs, GridFleaCollection& gridflea_ptrs)
{
    cout << "\nTest InFest::operator+=(const GridFlea& gridFlea)\n";

    GridFlea *activeGridFlea = find_nearest_active_gridflea(gridflea_ptrs);

    if (activeGridFlea == nullptr)
    {
        cout << "Re-run to perform test_infests_assign_add_gridflea() since there is no active gridflea object\n";
        cout << "\nTested InFest::operator+(const GridFlea& gridFlea)\t\t\t" << format_msg(false) << "\n";
        return;
    }

    cout << "\nAdd GridFlea with value: " << activeGridFlea->value() << "\n";

    for (std::size_t i = 0; i < infest_ptrs.size(); i++)
    {
        cout << "======================\n";
        int old_value = infest_ptrs[i]->value();
        int old_size = infest_ptrs[i]->size();
        cout << "InFest["<< i <<"] value before adding GridFlea: " << old_value << "\n";
        cout << "InFest["<< i <<"] size before adding GridFlea: " << old_size << "\n";

        *(infest_ptrs[i]) += *activeGridFlea;

        int new_value = infest_ptrs[i]->value();
        int new_size = infest_ptrs[i]->size();
        cout << "\nInFest["<< i <<"] value after adding GridFlea: " << new_value << "\n";
        cout << "InFest["<< i <<"] size after adding GridFlea: " << new_size << "\n";
    }

    cout << "\nTested InFest::operator+=(const GridFlea& gridFlea)\t" << format_msg(true) << "\n";
}

void test_gridfleas_jump_outside_inbound()
{
    cout << "\nTest GridFleas permittedly jump outside\n";

    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> grid_range(318, 320);
    std::uniform_int_distribution<std::size_t> size_range(2, 5);
    std::uniform_int_distribution<std::size_t> step_range(1, 8);

    GridFleaCollection fleas(SAMPLE);
    std::size_t step = step_range(generator);

    for (std::size_t i  = 0; i < fleas.size(); i++)
    {
        std::size_t x = grid_range(generator);
        std::size_t y = grid_range(generator);
        std::size_t size = size_range(generator);

        cout << "GridFlea(" << x << ", " << y << ", " << size << "):\n";

        fleas[i] = std::make_unique<GridFlea>(x,y,size);
        fleas[i]->move(step);

        std::string direction = (step % 2 == 0) ? "X" : "Y";
        std::size_t actual_step = (step % 2 == 0) ? step / 2 : step;

        cout << "\tFirst moved out of bound on "<< direction << ": " << actual_step << " steps\n";
        cout << "\tValue: " << fleas[i]->value() << "\n";

        fleas[i]->move(step);
        cout << "\tSecond moved out of bound on "<< direction << ": " << actual_step << " steps\n";
        cout << "\tValue: " << fleas[i]->value() << "\n";
    }

    cout << "\nTested GridFleas permittedly jump outside\t\t" << format_msg(true) << endl;
}

void test_gridfleas_jump_outside_inbound_with_reset()
{
    cout << "\nTest GridFleas permittedly jump outside with reset\n";

    std::random_device dev;
    std::mt19937 generator{dev()};
    std::uniform_int_distribution<std::size_t> grid_range(318, 320);
    std::uniform_int_distribution<std::size_t> size_range(2, 5);
    std::uniform_int_distribution<std::size_t> step_range(1, 8);

    GridFleaCollection fleas(SAMPLE);
    std::size_t step = step_range(generator);

    for (std::size_t i  = 0; i < fleas.size(); i++)
    {
        std::size_t x = grid_range(generator);
        std::size_t y = grid_range(generator);
        std::size_t size = size_range(generator);

        cout << "GridFlea(" << x << ", " << y << ", " << size << "):\n";

        fleas[i] = std::make_unique<GridFlea>(x,y,size);
        fleas[i]->move(step);

        std::string direction = (step % 2 == 0) ? "X" : "Y";
        int actual_step = (int)((step % 2 == 0) ? step / 2 : step);

        cout << "\tFirst moved out of bound on "<< direction << ": " << (actual_step) << " steps\n";
        cout << "\tValue: " << fleas[i]->value() << "\n";

        fleas[i]->move(-step);
        cout << "\tSecond moved out of bound on "<< direction << ": " << (-actual_step) << " steps\n";
        cout << "\tValue: " << fleas[i]->value() << "\n";

        fleas[i]->revive();
        cout << "\tValue after reset: " << fleas[i]->value() << "\n";

        fleas[i]->move(-step);
        cout << "\tThird moved out of bound on "<< direction << ": " << (-actual_step) << " steps\n";
        cout << "\tValue: " << fleas[i]->value() << "\n";

    }

    cout << "\nTested GridFleas permittedly jump outside with reset\t\t" << format_msg(true) << endl;
}
int main(int argc, char **argv)
{

    std::size_t initial_args[4][10];
    initialialize_args(initial_args);

    InFestCollection infest_ptrs(SAMPLE);
    // std::vector<std::unique_ptr<InFest>> infest_ptrs(SAMPLE);

    test_initialize(infest_ptrs, initial_args);
    test_min(infest_ptrs);
    test_max(infest_ptrs);

    InFestCollection new_infest_ptrs1(SAMPLE);
    InFestCollection new_infest_ptrs2(SAMPLE);
    InFestCollection new_infest_ptrs3(SAMPLE);

    test_copy(infest_ptrs, new_infest_ptrs1, initial_args);
    test_min(new_infest_ptrs1);
    test_max(new_infest_ptrs1);

    test_move(infest_ptrs);
    test_min(infest_ptrs);
    test_max(infest_ptrs);

    test_move_semantic(new_infest_ptrs1, new_infest_ptrs2, initial_args);
    test_min(new_infest_ptrs2);
    test_max(new_infest_ptrs1); // -2 means no elements

    test_move(new_infest_ptrs2);
    test_min(new_infest_ptrs2);
    test_max(new_infest_ptrs2);

    test_copy2(new_infest_ptrs2, new_infest_ptrs3);
    test_min(new_infest_ptrs3); // should match with new_infest2

    test_copy_assignment(infest_ptrs, new_infest_ptrs3);
    test_min(new_infest_ptrs3);

    test_move_copy_assignment(new_infest_ptrs2, new_infest_ptrs3);
    test_min(new_infest_ptrs2);
    test_min(new_infest_ptrs3);

    test_shared_ptr();
    test_shared_ptr_with_reset();

    GridFleaCollection gridfleas(SAMPLE);

    test_initialize_gridfleas(gridfleas, initial_args);

    // Before moving
    test_gridfleas_eqoperator(gridfleas, *gridfleas[0]);
    test_gridfleas_neoperator(gridfleas, *gridfleas[0]);
    test_gridfleas_gtoperator(gridfleas, *gridfleas[0]);
    test_gridfleas_geoperator(gridfleas, *gridfleas[0]);
    test_gridfleas_ltoperator(gridfleas, *(gridfleas[0]));
    test_gridfleas_leoperator(gridfleas, *gridfleas[0]);

    test_gridfleas_nonassigned_plusop(gridfleas);
    test_gridfleas_assigned_plusop(gridfleas);
    test_gridfleas_nonassigned_minusop(gridfleas);
    test_gridfleas_assigned_minusop(gridfleas);
    test_gridfleas_postincrement(gridfleas);
    test_gridfleas_preincrement(gridfleas);
    test_gridfleas_postdecrement(gridfleas);
    test_gridfleas_predecrement(gridfleas);

    // After moving
    GridFlea *activeGridFlea = find_nearest_active_gridflea(gridfleas);

    if (activeGridFlea != nullptr)
    {
        test_gridfleas_eqoperator(gridfleas, *activeGridFlea);
        test_gridfleas_neoperator(gridfleas, *activeGridFlea);
        test_gridfleas_gtoperator(gridfleas, *activeGridFlea);
        test_gridfleas_geoperator(gridfleas, *activeGridFlea);
        test_gridfleas_ltoperator(gridfleas, *activeGridFlea);
        test_gridfleas_leoperator(gridfleas, *activeGridFlea);
    }
    else
        cout << "Re-run to check GridFlea comparison operators since all objects are inactive\n";

    //InFestCollection infests(SAMPLE);
    //test_initialize(infests, initial_args);

    // Before moving
    test_infests_eqoperator(infest_ptrs, *infest_ptrs[0]);
    test_infests_neoperator(infest_ptrs, *infest_ptrs[0]);
    test_infests_gtoperator(infest_ptrs, *infest_ptrs[0]);
    test_infests_geoperator(infest_ptrs, *infest_ptrs[0]);
    test_infests_ltoperator(infest_ptrs, *infest_ptrs[0]);
    test_infests_leoperator(infest_ptrs, *infest_ptrs[0]);

    test_infests_nonassigned_plusop(infest_ptrs);
    test_infests_assigned_plusop(infest_ptrs);
    test_infests_nonassigned_minusop(infest_ptrs);
    test_infests_assigned_minusop(infest_ptrs);
    test_infests_postincrement(infest_ptrs);
    test_infests_preincrement(infest_ptrs);
    test_infests_postdecrement(infest_ptrs);
    test_infests_predecrement(infest_ptrs);

    // After moving
    test_infests_eqoperator(infest_ptrs, *infest_ptrs[0]);
    test_infests_neoperator(infest_ptrs, *infest_ptrs[0]);
    test_infests_gtoperator(infest_ptrs, *infest_ptrs[0]);
    test_infests_geoperator(infest_ptrs, *infest_ptrs[0]);
    test_infests_ltoperator(infest_ptrs, *infest_ptrs[0]);
    test_infests_leoperator(infest_ptrs, *infest_ptrs[0]);

    test_infests_nonassign_add_gridflea(infest_ptrs, gridfleas);
    test_infests_assign_add_gridflea(infest_ptrs, gridfleas);

    test_gridfleas_jump_outside_inbound();
    test_gridfleas_jump_outside_inbound_with_reset();

    return 0;
}
