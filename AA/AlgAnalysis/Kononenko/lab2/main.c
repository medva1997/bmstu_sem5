#include <stdlib.h>
#include <stdio.h>
#include "functions.h"
#include <time.h>



int main(int argc, char** argv)
{
    /*
    srand(time(NULL));
    int a = 0, b = 0;
    int sum = 0, mult = 0;
    unsigned long long int time1 = 0, time2 = 0, time = 0;
    for (int i = 0; i < 10000; i++)
    {
        a = RandInt(100000);
        b = RandInt(100000);
        time = tick();
        sum = a + b;
        time1 += tick() - time;

        time = tick();
        mult = a * b;
        time2 += tick() - time;
    }
    printf("time1 = %lld , time2 = %lld\n"
           "time2/time1 = %f", time1, time2, (float)time2/(float)time1);
    */

    FILE *out = fopen("out.txt", "a");
    fprintf(out, "*****************Начало экперимента*****************\n");
    for (int h = 100; h <=1000; h += 100)
    {
        for (int i = 0; i < 2; i++)
        {
            int n = h + i;
            matrix_t m1 = create_random_matrix(n, n),
                     m2 = create_random_matrix(n, n);

            unsigned long long int time1 = 0, time2 = 0, time3 = 0, time = 0;
            matrix_t m3;

            for (int i = 0; i < 100; i++)
            {
                m3 = matr_mult(m1, m2, &time);
                free_matrix(&m3);
                time1 += time;

                m3 = vinograd_mult(m1, m2, &time);
                free_matrix(&m3);
                time2 += time;

                m3 = vinograd_mult_o2(m1, m2, &time);
                free_matrix(&m3);
                time3 += time;
            }
            time1 /= 100 * 1e6;
            time2 /= 100 * 1e6;
            time3 /= 100 * 1e6;

            free_matrix(&m1);
            free_matrix(&m2);
            char buffer[500];
            snprintf(buffer,500, "Size = %dX%d, Time(ticks / 1e-6):\n"
                   "                   Std alg      : %lld\n"
                   "                   Vinograd     : %lld\n"
                   "                   Vinograd opt : %lld\n"
                   "----------------------------------------\n", n, n, time1, time2, time3);
            fprintf(stdout, buffer);
            fprintf(out, buffer);
        }
    }
    fprintf(out, "*****************Конец эксперимента*****************\n\n");
    fclose(out);
    fprintf(stdout, "done");

    return 0;
}
