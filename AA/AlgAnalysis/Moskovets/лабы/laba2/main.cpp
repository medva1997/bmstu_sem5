#include <iostream>
#include <fstream>
#include "matrix.h"
using namespace std;
typedef long long unsigned int TickType;
TickType tick(void)
{
    TickType d;

    __asm__ __volatile__ ("rdtsc" : "=A" (d) );

    return d;
}
void test(std::ofstream &stream, int n, int count)
{
    TickType tStand = 0, tVin = 0, tMod = 0;
    TickType tb, te;
    Matrix a(n, n), b(n, n);
    a.generateRandomElements();
    b.generateRandomElements();
    for(int i = 0; i < count; i++) {
        tb = tick();
        Matrix res1(multStandart(a, b));
        te = tick();
        tStand += (te - tb) / count;

        tb = tick();
        Matrix res2(multVinograd(a, b));
        te = tick();
        tVin += (te - tb) / count;

        tb = tick();
        Matrix res3(multModVinograd(a, b));
        te = tick();
        tMod += (te - tb) / count;
    }
    stream << n << "\t" << tStand << "\t" << tVin << "\t" << tMod << endl;
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
    Matrix res3(multModVinograd(a, b));

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
    for(int i = 100; i <= 1000; i += 100) {
        test(stream, i, 10);
        cout << i << endl;
    }

    for(int i = 101; i <= 1001; i += 100) {
        test(stream, i, 10);
        cout << i << endl;
    }
    stream.close();
    return 0;
}
