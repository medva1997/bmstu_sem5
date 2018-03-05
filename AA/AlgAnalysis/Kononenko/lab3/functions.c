#include <stdio.h>
#include <stdlib.h>
#include "functions.h"
#include <string.h>
#include <time.h>


int compar_int(const void *a, const void *b)
{
    return *(const int *)a - *(const int *)b;
}

int dec_compar_int(const void *a, const void *b)
{
    return *(const int *)b - *(const int *)a;
}

static void swap(void *p1, void *p2, size_t size)
{
    char *cp1 = p1;
    char *cp2 = p2;
    for (int i = 0; i < size; i++)
    {
        char tmp = *cp1;
        *cp1++ = *cp2;
        *cp2++ = tmp;
    }
}

//qsort
static void sort(void *const lp, void *const rp, size_t size,
    compare_t compare)
{
    void *n = malloc(size);
    memcpy(n, lp, size);
    void *i = lp, *j = rp;
    while (i < j)
    {
        while (compare(i, n) < 0)
            i = (char *)i + size;
        while (compare(j, n) > 0)
            j = (char *)j - size;
        if (i <= j)
        {
            if (compare(i, j) != 0)
                swap(i, j, size);
            i = (char *)i + size;
            j = (char *)j - size;
        }
    }
    free(n);
    if (i < rp)
        sort(i, rp, size, compare);
    if (lp < j)
        sort(lp, j, size, compare);
}

void my_qsort(void *arr, size_t count, size_t size,
              compare_t compare)
{
    void *lp = arr;
    void *rp = (char *)arr + size * (count - 1);
    sort(lp, rp, size, compare);
}

//bubble
void my_bubble_sort(void *arr, size_t count, size_t size,
                    compare_t compare)
{
    char *endp = (char *)arr + count * size;
    for (register int i = 0; i < count - 1; i++)
        for (char * j = arr + size; j < endp - (i * size); j+= size)
            if (compare(j - size, j) > 0)
                swap(j - size, j, size);
}

void my_selection_sort(void *arr, size_t count, size_t size,
                       compare_t compare)
{
    char *endp = (char *)arr + count * size;
    for (char *i = arr; i < endp - size; i += size)
    {
        size_t min = 0;
        for (char *j = i + size; j < endp; j += size)
            if (compare(i + (min * size), j) > 0)
                min = (j - i) / size;
        swap(i, i + (min * size), size);
    }
}

int is_sorted(int *arr, size_t count)
{
    for(int i = 1; i < count; i++)
        if (arr[i - 1] > arr[i])
            return 0;
    return 1;
}

unsigned long long int tick(void)
{
  unsigned long long int time = 0;
  __asm__ __volatile__ ("rdtsc" : "=A" (time));
  return time;
}

static double RandDbl(void)
{
    return rand() / ((double)RAND_MAX + 1.0);
}

int RandInt(int n)
{
    return (int)(n * RandDbl());
}

void randomize_arr(int *arr, size_t count)
{
    static int flag = 1;
    if (flag)
    {
        srand(time(NULL));
        flag = 0;
    }
    for(int i = 0; i < count; i++)
        arr[i] = RandInt(100);
}
