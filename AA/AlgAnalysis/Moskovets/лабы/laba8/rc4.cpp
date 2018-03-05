#include "rc4.h"

void RC4::swap(byte *arr, int i, int j)
{
    byte tmp = arr[i];
    arr[i] = arr[j];
    arr[j] = tmp;
}

RC4::RC4(unsigned char *key, size_t size)
{
    init(key, size);
}

void RC4::init(unsigned char *key, size_t size)
{
    for (int i = 0; i < 256; i++) {
        gamma[i] = (byte)i;
    }

    int j = 0;
    for (int i = 0; i < 256; i++) {
        j = (j + gamma[i] + key[i % size]) % 256;
        swap(gamma, i, j);
    }
    indI = indJ = 0;
}

byte RC4::kword()
{
    indI = (indI + 1) % 256;
    indJ = (indJ + gamma[indI]) % 256;
    swap(gamma, indI, indJ);
    byte K = gamma[(gamma[indI] + gamma[indJ]) % 256];
    return K;
}

char RC4::code(char ch)
{
    return (char)(ch ^ kword());
}
