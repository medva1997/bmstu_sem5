#ifndef RINGBUFFER_H
#define RINGBUFFER_H
#include <iostream>

template <typename T, size_t Size>
class RingBuffer
{
private:
    T buff[Size];
    size_t front;
    size_t back;
    size_t count;
public:
    RingBuffer()
        : front(0),
          back(0),
          count(0)
    {}

    bool push(T data)
    {
        if(isFull())
            return false;
        buff[front] = data;
        front++;
        count++;
        front %= Size;
        return true;
    }

    bool pop(T& x)
    {
        if(isEmpty())
            return false;
        x = buff[back];
        back++;
        count--;
        back %= Size;
        return true;
    }
    size_t getCount() const
    {
        return count;
    }

    bool isEmpty() const
    {
        return count == 0;
    }
    bool isFull() const
    {
        return count == Size;
    }
    void print() const
    {
        //std::cout << "Empty:" << isEmpty() << std::endl;
        //std::cout << "Full:"  << isFull()  << std::endl;
        //std::cout << "Count:" << getCount()<< std::endl;

        size_t i = back;
        if (count != 0)
        {
            do {
                std::cout << buff[i] << " ";
                i++;
                i %= Size;
            } while(i != front);
            std::cout << std::endl;
        }
        std::cout << std::endl;
    }
};

#endif // RINGBUFFER_H

