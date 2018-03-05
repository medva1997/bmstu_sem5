#include <iostream>
#include <fstream>
#include "matrix.h"
using namespace std;
typedef clock_t TickType;
/*typedef long long unsigned int TickType;
TickType tick(void)
{
    TickType d;

    __asm__ __volatile__ ("rdtsc" : "=A" (d) );

    return d;
}*/
void test(std::ofstream &stream, int n, int count)
{
    TickType tMod = 0, tArr[20] = {0};
    TickType tb, te;
    Matrix a(n, n), b(n, n);
    a.generateRandomElements();
    b.generateRandomElements();
    for(int i = 0; i < count; i++) {
        tb = clock();
        Matrix res3(multModVinograd(a, b));
        te = clock();
        tMod += (te - tb) / count;

        for(int j = 1; j <= 16; j *= 2) {
            tb = clock();
            Matrix res2(multThreadVinograd(a, b, j));
            te = clock();
            tArr[j] += (te - tb) / count;
        }
    }
    stream << n << "\t" << tMod;
    for(int j = 1; j <= 16; j *= 2) {
        stream << "\t" << tArr[j];
    }
    stream << endl;
}

int main()
{
    srand(0);
    int z = 11;
    int n = z, m = z, k = z;

    Matrix a(n, m), b(m, k);
    a.generateRandomElements();
    b.generateRandomElements();
    Matrix res1(multStandart(a, b));
    Matrix res2(multVinograd(a, b));
    Matrix res3(multThreadVinograd(a, b, 3));

    a.print();
    cout << "\n\n";
    b.print();
    cout << "\n\n";
    res1.print();
    cout << "\n\n";
    res2.print();
    cout << "\n\n";
    res3.print();

    if(res1 == res2) {
        cout << "yes1";
    }
    if(res3 == res2) {
        cout << "yes2";
    }

    std::ofstream stream;
    stream.open("res3.txt");
    for(int i = 100; i <= 500; i += 100) {
        test(stream, i, 10);
        cout << i << endl;
    }

    for(int i = 101; i <= 501; i += 100) {
        test(stream, i, 10);
        cout << i << endl;
    }
    stream.close();
    return 0;
}
