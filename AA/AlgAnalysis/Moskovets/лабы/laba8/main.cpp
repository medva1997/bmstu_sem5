#include "rc4.h"
#include "ringbuffer.h"

#include <iostream>
#include <cstring>
#include <thread>
#include <chrono>

using namespace std;

#define BUFSIZE 100

typedef unsigned long long int timeType;


void codeData(RingBuffer<char, BUFSIZE> &buff, timeType sleep, const char* str,
                                               unsigned char* key, size_t size)
{
    RC4 code(key, size);
    size_t i = 0;
    while(i < strlen(str)) {
        std::this_thread::sleep_for(std::chrono::milliseconds(sleep));

        if(!buff.isFull())
            buff.push(code.code(str[i++]));
    }
}
void decodeData(RingBuffer<char, BUFSIZE> &buff, timeType sleep, size_t strSize,
                                                 unsigned char* key, size_t size)
{
    RC4 decode(key, size);
    size_t i = 0;
    while(i < strSize) {
        std::this_thread::sleep_for(std::chrono::milliseconds(sleep));

        if(!buff.isEmpty()) {
            char x;
            buff.pop(x);
            x = decode.code(x);
            cout << x << flush;
            i++;
        }
    }
    cout << endl;
}
int main(int argc, char *argv[])
{
    RingBuffer<char, BUFSIZE> data;
    unsigned char K[] = { 2, 3, 1, 0, 3 };
    size_t KSize = 5;
    const char* str = "Hello World!";

    std::thread coder(&codeData, std::ref(data), 1, str, K, KSize);
    std::thread decoder(&decodeData, std::ref(data), 500, strlen(str), K, KSize);

    coder.join();
    decoder.join();
    return 0;
}
