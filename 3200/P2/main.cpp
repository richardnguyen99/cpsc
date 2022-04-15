#include <random>
#include <iostream>
#include "InFest.h"

using std::cout;
using std::endl;
using InFestPtr = InFest *;

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

void test_initialize(InFest **&infests, std::size_t initial_args[][10])
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
        infests[i] = new InFest(x, y, size, num);
    }

    cout << "Tested InFest initialization\t\t\t" << format_msg(status) << endl;
}

void test_min(InFest **&infests)
{
    cout << "\nTesting InFest::min()" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        cout << "\tInFests[" << i << "].min() = " << infests[i]->min() << endl;
    }

    cout << "Tested InFest::min()\t\t\t\t" << format_msg(status) << endl;
}

void test_max(InFest **&infests)
{
    cout << "\nTesting InFest::max()" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        cout << "\tInFests[" << i << "].max() = " << infests[i]->max() << endl;
    }

    cout << "Tested InFest::max()\t\t\t\t" << format_msg(status) << endl;
}

void test_move(InFest **&infests)
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
        infests[i]->move(step);
    }

    cout << "Tested InFest::move()\t\t\t\t" << format_msg(status) << endl;
}

void test_copy(InFest **&src, InFest **&dest, std::size_t initial_args[][10])
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
        dest[i] = new InFest(*(src[i]));
    }

    cout << "Tested InFest copy constructor\t\t\t" << format_msg(status) << endl;
}

void test_copy2(InFest **&src, InFest **&dest)
{
    cout << "\nTesting InFest copy constructor" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
        dest[i] = new InFest(*(src[i]));

    cout << "Tested InFest copy constructor\t\t\t" << format_msg(status) << endl;
}

void test_move_semantic(InFest **&src, InFest **&dest, std::size_t initial_args[][10])
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
        dest[i] = new InFest(std::move(*(src[i])));
    }

    cout << "Tested InFest move constructor\t\t\t" << format_msg(status) << endl;
}

void test_copy_assignment(InFest **&src, InFest **&dest)
{
    cout << "\nTesting InFest::operator=" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        *dest[i] = *src[i];
    }

    cout << "Tested InFes::operator=\t\t\t\t" << format_msg(status) << endl;
}

void test_move_copy_assignment(InFest **&src, InFest **&dest)
{
    cout << "\nTesting InFest::operator=(&&)" << endl;

    bool status = true;
    for (int i = 0; i < SAMPLE; i++)
    {
        *dest[i] = std::move(*src[i]);
    }

    cout << "Tested InFes::operator=(&&)\t\t\t\t" << format_msg(status) << endl;
}

int main(int argc, char **argv)
{

    std::size_t initial_args[4][10];
    initialialize_args(initial_args);

    InFestPtr *infests = new InFestPtr[SAMPLE];

    test_initialize(infests, initial_args);
    test_min(infests);
    test_max(infests);

    InFestPtr *new_infests1 = new InFestPtr[SAMPLE];
    InFestPtr *new_infests2 = new InFestPtr[SAMPLE];
    InFestPtr *new_infests3 = new InFestPtr[SAMPLE];

    test_copy(infests, new_infests1, initial_args);
    test_min(new_infests1);
    test_max(new_infests1);

    test_move(infests);
    test_min(infests);
    test_max(infests);

    test_move_semantic(new_infests1, new_infests2, initial_args);
    test_min(new_infests2);
    test_min(new_infests1); // -2 means no elements

    test_move(new_infests2);
    test_min(new_infests2);
    test_max(new_infests2);

    test_copy2(new_infests2, new_infests3);
    test_min(new_infests3); // should match with new_infest2

    test_copy_assignment(infests, new_infests3);
    test_min(new_infests3);

    test_move_copy_assignment(new_infests2, new_infests3);
    test_min(new_infests2);
    test_min(new_infests3);

    for (int i = 0; i < SAMPLE; i++)
    {
        delete infests[i];
        delete new_infests1[i];
        delete new_infests2[i];
        delete new_infests3[i];
    }

    delete[] infests;
    delete[] new_infests1;
    delete[] new_infests2;
    delete[] new_infests3;

    return 0;
}
