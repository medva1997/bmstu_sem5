#include <stdio.h>
#include <stdlib.h>
#include "functions.h"
#include <conio.h>

#define EXP_COUNT 100

int main(int argc, char** argv)
{
    FILE *out = fopen("out.txt", "a");
    fprintf(out, "*****************Начало экперимента*****************\n");
    for (int i = 100; i < 1001; i += 100)
    {
        unsigned long long int time1b = 0, time1w = 0, time1r = 0;
        unsigned long long int time2b = 0;
        unsigned long long int time3b = 0, time3w = 0, time = 0;
        for (int t = 0; t < EXP_COUNT; t++)
        {
            int *a = malloc(i * sizeof(int));


            //*************bubble_sort
            //best_case
            randomize_arr(a, i);
            qsort(a, i, sizeof(int), compar_int);

            time = tick();
            my_bubble_sort(a, i, sizeof(int), compar_int);
            time1b += tick() - time;

            //worst_case
            randomize_arr(a, i);
            qsort(a, i, sizeof(int), dec_compar_int);

            time = tick();
            my_bubble_sort(a, i, sizeof(int), compar_int);
            time1w += tick() - time;

            //random_case
            randomize_arr(a, i);

            time = tick();
            my_bubble_sort(a, i, sizeof(int), compar_int);
            time1r += tick() - time;

            //*************selection_sort
            //best_case = worst_case = random
            randomize_arr(a, i);

            time = tick();
            my_selection_sort(a, i, sizeof(int), compar_int);
            time2b += tick() - time;


            //*************qsort
            //best_case = random
            randomize_arr(a, i);

            time = tick();
            my_qsort(a, i, sizeof(int), compar_int);
            time3b += tick() - time;

            //worst_case
            randomize_arr(a, i);
            qsort(a, i, sizeof(int), compar_int);

            time = tick();
            my_qsort(a, i, sizeof(int), compar_int);
            time3w += tick() - time;
        }
        time1b /= EXP_COUNT * 1e3;
        time1w /= EXP_COUNT * 1e3;
        time1r /= EXP_COUNT * 1e3;
        time2b /= EXP_COUNT * 1e3;
        time3b /= EXP_COUNT * 1e3;
        time3w /= EXP_COUNT * 1e3;

        char buffer[500];
        snprintf(buffer,500, "Size = %d, Time(ticks / 1e3):\n"
               "                   Bubble best    : %lld\n"
               "                   Bubble random  : %lld\n"
               "                   Bubble worst   : %lld\n"
               "-----------------------\n"
               "                   Select best    : %lld\n"
               "-----------------------\n"
               "                   Qsort best     : %lld\n"
               "                   Qsort worst    : %lld\n"
               "----------------------------------------\n", i, time1b, time1r, time1w,
                 time2b, time3b, time3w);
        fprintf(stdout, buffer);
        fprintf(out, buffer);

    }
    fprintf(out, "*****************Конец эксперимента*****************\n\n");
    fclose(out);
    fprintf(stdout, "done");

    getch();
    return 0;
}

