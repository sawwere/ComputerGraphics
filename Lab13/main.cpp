#include <iostream>


int main()
{
    int taskCode = 0;
    while (true)
    {
        std::cout << "Enter task number: ";
        std::cin >> taskCode;
        switch (taskCode)
        {
        case 0:
            return 0;
        default:
            continue;;
        }
    }
    return 0;
}