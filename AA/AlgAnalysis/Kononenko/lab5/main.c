#include <stdlib.h>
#include <stdio.h>
#include "functions.h"
#include <time.h>
#include <math.h>
#include <conio.h>

#define RUN_CNT 20
#define QUEUE_LENGTH 20000
int main(int argc, char** argv)
{
    unsigned long long time1 = 0, time2 = 0, time = 0;
    //srand(time(NULL));
    for (int i = 0; i < RUN_CNT; i++)
    {
        queue_t q1 = create_queue(),
                q2 = create_queue();
        push_random_q(&q1, QUEUE_LENGTH);
        push_random_q(&q2, QUEUE_LENGTH);

        process_data(&q1, &time);
        time1 += time;

        process_data_par(&q2, &time);
        time2 += time;

        free_q(&q1);
        free_q(&q2);
    }
    time1 /= RUN_CNT * 1e6;
    time2 /= RUN_CNT * 1e6;

    printf("One Thread    : %lld\n"
           "Three Threads : %lld\n", time1, time2);

    return 0;
}
