#include <stdlib.h>
#include <stdio.h>
#include "functions.h"
#include <time.h>

int main(int argc, char** argv)
{
    int n = 100;
    matrix_t m = create_random_matrix(n);

    //print_matr(m);

    int q = 50;
    solution_t sol;
    for (float i = 0; i <= 1.01; i+= 0.1)
    {

        sol = solve(m, i, 1-i, 0.5, q, 200);

        printf("alpha          :%.1f\n"
               "beta           :%.1f\n"
               "Length         :%d\n"
               "-----------------------\n",i, 1-i, sol.l);
    }
    for (int i = 100; i <= 1000; i+= 100)
    {

        sol = solve(m, 0.5, 0.5, 0.5, q, i);

        printf("LifeTime       :%d\n"
               "Length         :%d\n"
               "-----------------------\n",i, sol.l);
    }
    free_array(&sol.route);
    return 0;
}
