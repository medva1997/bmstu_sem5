#include <stdlib.h>
#include <stdio.h>
#include "functions.h"
#include <string.h>
#define STRLEN 100



int main(int argc, char** argv)
{
    char* string1 = malloc(STRLEN);
    char* string2 = malloc(STRLEN);
    fprintf(stdout, "Input 1-st string:\n");
    read_string(string1);
    //fprintf(stdout, "length = %d\n", strlen(string1));

    fprintf(stdout, "Input 2-nd string:\n");
    read_string(string2);
    //fprintf(stdout, "length = %d\n", strlen(string2));
    unsigned long long int t = 0, time1 = 0, time2 = 2, time3 = 0;
    int dist1, dist2, dist3;
    for (int i= 0; i < 1000; i++)
    {
        dist1 = Levenstein_simple(string1, string2, &t);
        time1 += t;

        dist2 = Levenstein_Damer(string1, string2, &t);
        time2 += t;

        t = tick();
        dist3 = Levenstein_r(string1, string2);
        t = tick() - t;
        time3 += t;
    }
    time1 /= 1000;
    time2 /= 1000;
    time3 /= 1000;

    free(string1);
    free(string2);

    fprintf(stdout, "Levenstein dist         = %d, time = %d\n", dist1, time1);
    fprintf(stdout, "Damerau-Levenstein dist = %d, time = %d\n", dist2, time2);
    fprintf(stdout, "Levenstein rec.    dist = %d, time = %d\n", dist3, time3);
    return 0;
}
