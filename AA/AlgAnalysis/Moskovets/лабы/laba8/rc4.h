#ifndef RC4_H
#define RC4_H
#include <iostream>

typedef unsigned char byte;

class RC4
{
    byte gamma[256];
    int indI;
    int indJ;

    void swap(byte* arr, int i, int j);
public:
    RC4(unsigned char* key, size_t size);
    void init(unsigned char* key, size_t size);
    byte kword ();
    char code(char ch);
};

#endif // RC4_H
