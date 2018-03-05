#include <iostream>
#include <fstream>
#include <algorithm>
#include <ctime>
#define RAND_RANGE 1000
using namespace std;
typedef long long unsigned int TickType;
TickType tick(void)
{
    TickType d;

    __asm__ __volatile__ ("rdtsc" : "=A" (d) );

    return d;
}
template <class T>
void insertionSort(T* begin, T* end) {
    T tmp;
    T* j;
    for(T* i = begin + 1; i < end; i++) {
        tmp = *i;
        j = i - 1;
        while(j >= begin && tmp >= *j) {
            *(j + 1) = tmp;
            j--;
        }
    }
}

/*
 *             for(j = i - 1; j >= begin; j--) {
            if(tmp >= *j) {
                break;
            }
            *(j + 1) = *j;

 * */
template <class T>
void bubleSort(T* begin, T* end)
{
    bool flag;
    for (T *i = end - 1; i >= begin; i--) {
        flag = true;
        for (T *j = begin + 1; j <= i; j++) {
            if (*(j-1) > *j) {
                T tmp = *j;
                *j = *(j-1);
                *(j-1) = tmp;
                flag = false;
            }
        }
        if(flag)
            break;
    }
}
template <class T>
void quickSort(T* begin, T* end) {
    T *i = begin, *j = end, x = *(i + (j - i) / 2);

    do {
        while (*i < x) i++;
        while (*j > x) j--;

        if(i <= j) {
            if (*i > *j) {
                T tmp = *j;
                *j = *i;
                *i = tmp;
            }
            i++;
            j--;
        }
    } while (i <= j);

    if (i < end)
        quickSort(i, end);
    if (begin < j)
        quickSort(begin, j);
}


template <class T>
void generateRandomElements(T* arr, int n)
{
    for(int i = 0; i < n; i++) {
       arr[i] = ((double) rand() / RAND_MAX) * RAND_RANGE;
    }
}

template <class T>
void copyArray(const T* arr, int n, T* copy)
{
    for(int i = 0; i < n; i++) {
       copy[i] = arr[i];
    }
}
template <class T>
void reverseArray(const T* arr, int n, T* copy)
{
    for(int i = 0; i < n; i++) {
       copy[i] = arr[n - i - 1];
    }
}

template <class T>
void print(T* arr, int n)
{
    for(int i = 0; i < n; i++) {
       cout << arr[i] << " ";
    }
    cout << endl;
}
enum TestFlag {
    SORTED,
    REVERSE,
    RANDOM
};

template <class T>
void testArr(std::ofstream &stream, int n, int count, TestFlag flag)
{
    T arr[n], generateArr[n];
    TickType tBuble = 0, tQuick = 0, tInsert = 0;
    TickType tb, te;
    for(int i = 0; i < count; i++) {
        generateRandomElements(generateArr, n);
        if(flag == SORTED || flag == REVERSE) {
             quickSort<int>(generateArr, generateArr + n);
        }
        if(flag == REVERSE) {
            reverse(generateArr, generateArr + n);
        }
        //print(generateArr, n);

        copyArray<T>(generateArr, n, arr);
        tb = tick();
        bubleSort<int>(arr, arr + n);
        te = tick();
//        tBuble += ((te > tb) ? (te - tb) : (tb - te)) / count;
        tBuble += (te - tb);

        copyArray<T>(generateArr, n, arr);
        tb = tick();
        insertionSort<int>(arr, arr + n);
        te = tick();
//        tInsert += ((te > tb) ? (te - tb) : (tb - te)) / count;
        tInsert += (te - tb);

        copyArray<T>(generateArr, n, arr);
        tb = tick();
        quickSort<int>(arr, arr + n);
        te = tick();
//        tQuick += ((te > tb) ? (te - tb) : (tb - te)) / count;
        tQuick += (te - tb);
    }
    stream << n << "\t" << tBuble/count << "\t" << tQuick/count << "\t" << tInsert/count << endl;
}

int main(int argc, char *argv[])
{
    //cout << tick() << endl;
    int n = 10;
    int* a = new int[n];
    generateRandomElements(a, n);
    print(a, n);

    quickSort<int>(a, a + n);
    print(a, n);

    generateRandomElements(a, n);
    print(a, n);

    insertionSort<int>(a, a + n);
    print(a, n);

    generateRandomElements(a, n);
    print(a, n);

    bubleSort<int>(a, a + n);
    print(a, n);

/*    std::ofstream stream;
    stream.open("res_sort6.txt");
    int count = 20;

    stream << "sorted" << endl;
    for(int i = 100; i <= 5000; i += 100) {
        cout << i << endl;
        testArr<int>(stream, i, count, SORTED);
    }

    stream << "reversed" << endl;
    for(int i = 100; i <= 5000; i += 100) {
        cout << i << endl;
        testArr<int>(stream, i, count, REVERSE);
    }

    stream << "random" << endl;
    for(int i = 100; i <= 5000; i += 100) {
        cout << i << endl;
        testArr<int>(stream, i, count, RANDOM);
    }
//    testArr<int>(stream, 10, 1, SORTED);
//    testArr<int>(stream, 10, 1, REVERSE);
//    testArr<int>(stream, 10, 1, RANDOM);
    stream.close(); //*/
    return 0;


}
