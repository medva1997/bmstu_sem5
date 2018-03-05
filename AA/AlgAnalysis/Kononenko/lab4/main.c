#include <stdlib.h>
#include <stdio.h>
#include "functions.h"
#include <math.h>
#include <conio.h>

#define RUN_CNT 10

int main(int argc, char** argv)
{
    char answ = 0;
    fprintf(stdout, "Choose experiment:\n"
                    "1. Std VS Par\n"
                    "2. Count of threads battle\n");
    answ = getch();
    FILE *out = fopen("out.txt", "a");
    fprintf(out, "*****************Начало экперимента*****************\n");
    if (answ == '1')
    {
        fprintf(out, "                    Std VS Par\n");
        for (int n = 100; n <=1000; n += 100)
        {
            matrix_t m1 = create_random_matrix(n, n),
                     m2 = create_random_matrix(n, n);

            unsigned long long int time1 = 0, time2 = 0, time3 = 0,
                                   time4 = 0, time = 0;
            matrix_t m3;

            for (int i = 0; i < RUN_CNT; i++)
            {
                m3 = matr_mult(m1, m2, &time);
                free_matrix(&m3);
                time1 += time;

                m3 = vinograd_mult(m1, m2, &time);
                free_matrix(&m3);
                time2 += time;

                m3 = matr_mult_par(m1, m2, &time, 4);
                free_matrix(&m3);
                time3 += time;

                m3 = vinograd_mult_par(m1, m2, &time, 4);
                free_matrix(&m3);
                time4 += time;
            }
            time1 /= RUN_CNT * 1e6;
            time2 /= RUN_CNT * 1e6;
            time3 /= RUN_CNT * 1e6;
            time4 /= RUN_CNT * 1e6;

            free_matrix(&m1);
            free_matrix(&m2);
            char buffer[500];
            snprintf(buffer,500, "Size = %dX%d, Time(ticks / 1e-6):\n"
                   "                   Std alg      : %lld\n"
                   "                   Vinograd     : %lld\n"
                   "                   Std par      : %lld\n"
                   "                   Vinograd par : %lld\n"
                   "----------------------------------------\n", n, n, time1, time2, time3, time4);
            fprintf(stdout, buffer);
            fprintf(out, buffer);
        }
    }
    if (answ == '2')
    {
        fprintf(out, "                Count of threads battle\n");
        int n = 400;
        matrix_t m1 = create_random_matrix(n, n),
                 m2 = create_random_matrix(n, n);

        unsigned long long int times[5] = {0,0,0,0,0} , time = 0;
        matrix_t m3;
        for (int t = 0; t < 5; ++t)
        {
            for (int i = 0; i < RUN_CNT; i++)
            {
                m3 = vinograd_mult_par(m1, m2, &time, pow(2, t));
                free_matrix(&m3);
                times[t] += time;
            }
            times[t] /= RUN_CNT * 1e6;
        }

        free_matrix(&m1);
        free_matrix(&m2);
        char buffer[500];
        snprintf(buffer,500, "Num_of-threads Time(ticks / 1e-6):\n"
               "                   1         : %lld\n"
               "                   2         : %lld\n"
               "                   4         : %lld\n"
               "                   8         : %lld\n"
               "                   16        : %lld\n"

               "----------------------------------------\n", times[0], times[1], times[2], times[3], times[4]);
        fprintf(stdout, buffer);
        fprintf(out, buffer);

    }
    fprintf(out, "*****************Конец эксперимента*****************\n\n");
    fclose(out);
    fprintf(stdout, "done");

    return 0;
}
