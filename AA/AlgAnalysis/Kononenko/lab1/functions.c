#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include "functions.h"

#define BUFFSIZE 101

unsigned long long int tick(void)
{
  unsigned long long int time = 0;
  __asm__ __volatile__ ("rdtsc" : "=A" (time));
  return time;
}

int read_string(char* out)
{
    //fgets(out, count, stdin);
    *out = fgetc(stdin);
    for (; *out != '\0' && *out != '\n' && ++out;)
    {
        *out = fgetc(stdin);
    }
    *out = '\0';
    return 0;
}

static int **allocate_matrix(const size_t n, const size_t m)
{
    if (n == 0 || m == 0)
        return NULL;

    int **matr = calloc(n * sizeof(int *) + n * m * sizeof(int), 1);
    for (int i = 0; i < n; ++i)
        matr[i] = (int *)((char *)matr + n * sizeof(int *) + i * m * sizeof(int));
    return matr;
}

static void print_matr(int ** const matr, const int n, const int m)
{
    for (register int i = 0; i < n; ++i)
    {
        for (register int j = 0; j < m; ++j)
            printf("%d ", matr[i][j]);
        printf("\n");
    }
}

static int max(const int a, const int b)
{
    return a > b ? a : b;
}

static int max3(const int a, const int b, const int c)
{
    return a > b ? (a > c ? a : c) : (b > c ? b : c); //тернарная магия
}

static int min(const int a, const int b)
{
    return a < b ? a : b;
}

static int min3(const int a, const int b, const int c)
{
    return a < b ? (a < c ? a : c) : (b < c ? b : c); //тернарная магия
}

static int min4(const int a, const int b, const int c, const int d)
{
    int t1 = a < b ? a : b;
    int t2 = c < d ? c : d;
    return t1 < t2 ? t1 : t2;
}

int Levenstein_simple(const char* const  s1, const char* const s2, unsigned long long int *t)
{
    int len1 = strlen(s1);
    int len2 = strlen(s2);

    if (len1 == 0 || len2 == 0)
        return (max(len1, len2));

    int n = len1 + 1, m = len2 + 1;
    int** matr = allocate_matrix(n, m);

    *t = tick();
    for (register int i = 0; i <= len1; ++i)
        matr[i][0] = i;
    for (register int i = 0; i <= len2; ++i)
         matr[0][i] = i;

    for (register int i = 1; i < n; ++i)
        for (register int j = 1; j < m; ++j)
        {
            matr[i][j] = min3(matr[i][j - 1] + 1, matr[i - 1][j] + 1, matr[i-1][j-1] +
                    (s1[i-1] == s2[j-1] ? 0 : 1));
        }

    int result = matr[len1][len2];
    *t = tick() - *t;

    free(matr);
    return result;
}

int Levenstein_Damer(const char* const  s1, const char* const s2, unsigned long long *t)
{
    int len1 = strlen(s1);
    int len2 = strlen(s2);

    if (len1 == 0 || len2 == 0)
        return (max(len1, len2));

    int n = len1 + 1, m = len2 + 1;
    int** matr = allocate_matrix(n, m);

    *t = tick();
    for (register int i = 0; i <= len1; ++i)
        matr[i][0] = i;
    for (register int i = 1; i <= len2; ++i)
         matr[0][i] = i;

    for (register int i = 1; i < n; ++i)
        for (register int j = 1; j < m; ++j)
        {
            //проверка условия change
            if (i > 1 && j > 1 && s1[i-1] == s2[j-2] && s1[i-2] == s2[j-1])
                matr[i][j] = min4(matr[i][j - 1] + 1, matr[i - 1][j] + 1,
                    matr[i-1][j-1] + (s1[i-1] == s2[j-1] ? 0 : 1), matr[i-2][j-2] + 1);
            else
                matr[i][j] = min3(matr[i][j - 1] + 1, matr[i - 1][j] + 1,
                    matr[i-1][j-1] + (s1[i-1] == s2[j-1] ? 0 : 1));
        }
    int result = matr[len1][len2];
    *t = tick() - *t;
    free(matr);
    return result;
}

static int l_r(const char * const s1, const char * const s2, const int i, const int j)
{
    //т.к. нумерация с 0
    if (i < 0 || j < 0)
        return max(i + 1, j + 1);

    return min3(l_r(s1, s2, i-1, j) +1 , l_r(s1, s2, i, j-1) + 1,
                l_r(s1, s2, i-1, j-1) + (s1[i] == s2[j] ? 0 : 1));
}

int Levenstein_r(const char * const s1, const char * const s2)
{
    return l_r(s1, s2, strlen(s1), strlen(s2));
}
