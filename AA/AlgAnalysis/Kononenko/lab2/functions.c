#include <stdlib.h>
#include <stdio.h>
#include "functions.h"
#include <time.h>

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

static int **allocate_matrix(const size_t n, const size_t m)
{
    if (n == 0 || m == 0)
        return NULL;

    int **matr = calloc(n * sizeof(int *) + n * m * sizeof(int), 1);
    if (matr == NULL)
        return NULL;
    for (int i = 0; i < n; ++i)
        matr[i] = (int *)((char *)matr + n * sizeof(int *) + i * m * sizeof(int));

    return matr;
}

matrix_t create_matrix(const size_t n, const size_t m)
{

    matrix_t matrix;
    matrix.matr = allocate_matrix(n, m);
    if (matrix.matr == NULL)
    {
        matrix.n = 0;
        matrix.m = 0;
    }
    else
    {
        matrix.n = n;
        matrix.m = m;
    }
    return matrix;
}

void free_matrix(matrix_t *matrix)
{
    free(matrix->matr);
    matrix->m = 0;
    matrix->n = 0;
}

void print_matr(const matrix_t matrix)
{
    for (register int i = 0; i < matrix.n; ++i)
    {
        for (register int j = 0; j < matrix.m; ++j)
            printf("%d ", matrix.matr[i][j]);
        printf("\n");
    }
}

matrix_t create_random_matrix(const size_t n, const size_t m)
{
    static int flag = 1;
    if (flag)
    {
        srand(time(NULL));
        flag = 0;
    }
    matrix_t matrix;
    matrix.matr = allocate_matrix(n, m);
    if (matrix.matr == NULL)
    {
        matrix.n = 0;
        matrix.m = 0;
    }
    else
    {
        matrix.n = n;
        matrix.m = m;
        for (int i = 0; i < n; i ++)
            for (int j = 0; j < m; j++)
                matrix.matr[i][j] = RandInt(10);
    }
    return matrix;
}

matrix_t matr_mult(const matrix_t m1, const matrix_t m2, unsigned long long int *t)
{
    matrix_t res;
    if (m1.m != m2.n)
    {
        res = create_matrix(0, 0);
        return res;
    }

    res = create_matrix(m1.n, m2.m);
    *t = tick();
    for (int i = 0; i < m1.n; i++)
        for (int j = 0; j < m2.m; j++)
        {
            res.matr[i][j] = 0;
            for (int k = 0; k < m1.m; k ++)
                res.matr[i][j] += m1.matr[i][k] * m2.matr[k][j];
        }
    *t = tick() - *t;
    return res;
}

matrix_t vinograd_mult(const matrix_t m1, const matrix_t m2, unsigned long long int *t)
{
    matrix_t res;
    if (m1.m != m2.n)
    {
        res = create_matrix(0, 0);
        return res;
    }

    res = create_matrix(m1.n, m2.m);

    int *mul1 = malloc(sizeof(int) * m1.n ),
        *mul2 = malloc(sizeof(int) * m2.m);

    *t = tick();
    for (int i = 0; i < m1.n; i++)
    {
        mul1[i] = m1.matr[i][0] * m1.matr[i][1];
        for (int j = 1; j < m1.m / 2; j++)
            mul1[i] += m1.matr[i][2 * j] * m1.matr[i][2 * j + 1];
    }

    for (int j = 0; j < m2.m; j++)
    {
        mul2[j] = m2.matr[0][j] * m2.matr[1][j];
        for (int i = 1; i < m1.n / 2; i++)
            mul2[j] += m2.matr[2 * i][j] * m2.matr[2 * i + 1][j];
    }

    for (int i = 0; i < m1.n; i++)
        for (int j = 0; j < m2.m; j++)
        {
            res.matr[i][j] = -mul1[i] - mul2[j];
            for (int k = 0; k < m1.m / 2; k ++)
                res.matr[i][j] += (m1.matr[i][2*k+1] + m2.matr[2*k][j]) *
                                  (m1.matr[i][2*k] + m2.matr[2*k+1][j]);
        }

    if (m1.m  % 2 == 1)
        for (int i = 0; i < m1.n; i++)
            for (int j = 0; j < m2.m; j++)
                res.matr[i][j] += m1.matr[i][m1.m - 1]* m2.matr[m2.n - 1][j];
    *t = tick() - *t;

    free(mul1);
    free(mul2);
    return res;
}

matrix_t vinograd_mult_o2(const matrix_t m1, const matrix_t m2, unsigned long long int *t)
{
    matrix_t res;
    if (m1.m != m2.n)
    {
        res = create_matrix(0, 0);
        return res;
    }

    res = create_matrix(m1.n, m2.m);

    int *mul1 = malloc(sizeof(int) * m1.n ),
        *mul2 = malloc(sizeof(int) * m2.m);

    *t = tick();
    int tmp1 = m1.m - 1, tmp2 = m1.n - 1;
    for (int i = 0; i < m1.n; i++)
    {
        mul1[i] = m1.matr[i][0] * m1.matr[i][1];
        for (register int j = 2; j < tmp1; j += 1)

            mul1[i] += m1.matr[i][j] * m1.matr[i][j + 1];
    }

    for (int j = 0; j < m2.m; j++)
    {
        mul2[j] = m2.matr[0][j] * m2.matr[1][j];
        for (register int i = 2; i < tmp2; i += 2)
            mul2[j] += m2.matr[i][j] * m2.matr[i + 1][j];
    }
    if (m1.m % 2 == 0)
    {
        for (register int i = 0; i < m1.n; i++)
            for (register int j = 0; j < m2.m; j++)
            {
                res.matr[i][j] = -mul1[i] - mul2[j];
                for (register int k = 0; k < tmp1; k += 2)
                    res.matr[i][j] += (m1.matr[i][k+1] + m2.matr[k][j]) *
                                      (m1.matr[i][k] + m2.matr[k+1][j]);
            }
    }
    else
    {
        for (register int i = 0; i < m1.n; i++)
            for (register int j = 0; j < m2.m; j++)
            {
                res.matr[i][j] = -mul1[i] - mul2[j] + m1.matr[i][tmp1]* m2.matr[tmp2][j];
                for (register int k = 0; k < tmp1; k += 2)
                    res.matr[i][j] += (m1.matr[i][k+1] + m2.matr[k][j]) *
                                      (m1.matr[i][k] + m2.matr[k+1][j]);
            }
    }
    *t = tick() - *t;

    free(mul1);
    free(mul2);
    return res;
}

