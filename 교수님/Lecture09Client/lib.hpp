#pragma once
#include <conio.h>

char getKey()
{
    if (_kbhit())
    {        //키보드 입력 확인 (true / false)
        char c = _getch();

        if (c == 't')
        {
        }
    }

    return NULL;
}