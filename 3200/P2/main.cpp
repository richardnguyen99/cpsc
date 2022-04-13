#include <iostream>
#include "InFest.h"

using std::cout;
using std::endl;

int main(int argc, char **argv)
{
    InFest infest1(120, 120, 5, 10);
    InFest infest2(infest1);
    InFest infest3(128, 128, 4, 10);
    InFest infest4(std::move(infest3));

    cout << "Hello, World" << endl;

    cout << infest1.value() << endl;

    infest1.move(1);

    cout << infest1.value() << endl;

    return 0;
}
