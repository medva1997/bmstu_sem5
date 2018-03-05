#include <stdlib.h>
#include <stdio.h>
#include "functions.h"
#include <time.h>
#include <limits.h>
#include <math.h>

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

array_t create_array(size_t size)
{
    array_t array;
    array.arr = malloc(sizeof(float) * size);
    if (array.arr == NULL)
        array.size = 0;
    else
        array.size = size;
    return array;
}

static void copy_array(array_t dst, const array_t src)
{
    if (dst.size == src.size)
    {
        for (int i = 0; i < dst.size; i++)
            dst.arr[i] = src.arr[i];
    }
}

void print_array(FILE *out, const array_t a)
{
    for (int i = 0; i < a.size; i++)
        fprintf(out, "%.2f ", a.arr[i]);
    fprintf(out, "\n");
}


void free_array(array_t *a)
{
    free(a->arr);
    a->arr = NULL;
    a->size = 0;
}

static float **allocate_matrix(const size_t n, const size_t m)
{
    if (n == 0 || m == 0)
        return NULL;

    float **matr = calloc(n * sizeof(float *) + n * m * sizeof(float), 1);
    if (matr == NULL)
        return NULL;
    for (int i = 0; i < n; ++i)
        matr[i] = (float *)((char *)matr + n * sizeof(float *) + i * m * sizeof(float));

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
            printf("%6.2f", matrix.matr[i][j]);
        printf("\n");
    }
}

matrix_t create_random_matrix(const size_t n)
{
    static int flag = 1;
    if (flag)
    {
        srand(time(NULL));
        flag = 0;
    }
    matrix_t matrix;
    matrix.matr = allocate_matrix(n, n);
    if (matrix.matr == NULL)
    {
        matrix.n = 0;
        matrix.m = 0;
    }
    else
    {
        matrix.n = n;
        matrix.m = n;
        for (int i = 0; i < n; i ++)
            for (int j = i; j < n; j++)
                if (i == j)
                    matrix.matr[i][j] = 0;
                else
                {
                    matrix.matr[i][j] = RandInt(30) + 1;
                    matrix.matr[j][i] = matrix.matr[i][j];
                }
    }
    return matrix;
}
static ant_t create_ant(int start_pos, int num_of_cities)
{
    ant_t ant;
    ant.start_city = start_pos;
    ant.curr_city = start_pos;
    ant.Lk = 0;
    ant.route = create_array(num_of_cities);
    ant.Jk = create_array(num_of_cities);
    return ant;
}

static void init_ant(ant_t *ant)
{
    int start = ant->start_city;
    ant->curr_city = start;
    ant->Lk = 0;
    for (int i = 0; i < ant->route.size; i++)
    {
        ant->route.arr[i] = -1;
        ant->Jk.arr[i] = 1;
    }
    ant->route.arr[0] = start;
    ant->Jk.arr[start] = 0;
}

static ant_t *create_ant_array(int num_of_cities)
{
    ant_t *ant_arr = malloc(sizeof(ant_t) * num_of_cities);
    for (int i = 0; i < num_of_cities; i++)
        ant_arr[i] = create_ant(i, num_of_cities);
    return ant_arr;
}

static void free_ant(ant_t *ant)
{
    free_array(&ant->Jk);
    free_array(&ant->route);
}

static void free_ant_array(ant_t **ants, size_t n)
{
    for (int i = 0; i < n; i++)
        free_ant(&(*ants)[i]);
    free(*ants);
    *ants = NULL;
}

static void recalc_weight(matrix_t *weight, matrix_t pheromon, matrix_t visib, float a, float b)
{
    int N = weight->n;
    for (int i = 0; i < N; i++)
        for (int j = 0; j < N; j++)
            weight->matr[i][j] = pow(pheromon.matr[i][j], a) * pow(visib.matr[i][j], b);
}

static void recalc_pheromon(matrix_t *pheromon, matrix_t *d_pheromon, float p)
{
    p = 1 - p;
    int N = pheromon->n;
    for (int i = 0; i < N; i++)
        for (int j = 0; j < N; j++)
        {
            pheromon->matr[i][j] = pheromon->matr[i][j] * p + d_pheromon->matr[i][j];
            d_pheromon->matr[i][j] = 0;
        }
}

static int choose_next(array_t prob)
{
    int i = 0;
    float rand = RandDbl();
    if (rand == 0)
    {
        while (prob.arr[i++] <= 0);
        return --i;
    }
    while (rand > 0)
        rand -= prob.arr[i++];

    return --i;
}

static int length_of_route(matrix_t adj_mat, array_t route)
{
    int length = 0;
    for (int i = 0; i < route.size - 1; i++)
        length += adj_mat.matr[(int)route.arr[i]][(int)route.arr[i + 1]];
    return length;
}

static void add_pheromon(matrix_t d_pheromon, array_t route, int Lk, int q)
{
    float d_fer = (float)q / (float)Lk;
    for (int i = 0; i < route.size - 1; i++)
        d_pheromon.matr[(int)route.arr[i]][(int)route.arr[i + 1]] += d_fer;
}

static void gogo_ant(ant_t *ant, matrix_t adj_mat, matrix_t weight, matrix_t d_pheromon, int q)
{
    int N = weight.n;
    int i = 1;
    int next = 0;
    array_t prob = create_array(N);
    while (i < N)
    {

        float sum_weight = 0;
        for (int j = 0; j < N; j++)
            sum_weight += weight.matr[ant->curr_city][j] * ant->Jk.arr[j];
        for (int j = 0; j < N; j++)
        {
            prob.arr[j] = weight.matr[ant->curr_city][j] / sum_weight * ant->Jk.arr[j];
        }
        next = choose_next(prob);
        ant->curr_city = next;
        ant->Jk.arr[next] = 0;
        ant->route.arr[i++] = next;
    }
    ant->Lk = length_of_route(adj_mat, ant->route);
    add_pheromon(d_pheromon, ant->route, ant->Lk, q);
}



solution_t solve(matrix_t adj_mat, float a, float b, float p, int q, int t_max)
{
    static int flag = 1;
    if (flag)
    {
        srand(time(NULL));
        flag = 0;
    }

    int N = adj_mat.n;

    ant_t *ants = create_ant_array(N); //array of ants, 1 per city;

    matrix_t pheromon = create_matrix(N, N);
    for (int i = 0; i < N; i++)
        for (int j = 0; j < N; j++)
            pheromon.matr[i][j] = 0.5;

    matrix_t visib = create_matrix(N, N);
    for (int i = 0; i < N; i++)
        for (int j = i; j < N; j++)
            if (i != j)
            {
                visib.matr[i][j] = 1 / adj_mat.matr[i][j];
                visib.matr[j][i] = visib.matr[i][j];
            }
            else
                visib.matr[i][j] = 666;

    matrix_t weight = create_matrix(N,N);
    recalc_weight(&weight, pheromon, visib, a, b);

    matrix_t d_pheromon = create_matrix(N, N);
    for (int i = 0; i < N; i++)
        for (int j = 0; j < N; j++)
            d_pheromon.matr[i][j] = 0;

    int best_l = INT_MAX;
    array_t route = create_array(N);

    for (int t = 0; t < t_max; t++)
    {
        for (int k = 0; k < N; k++)
        {
            init_ant(&ants[k]);
            gogo_ant(&ants[k], adj_mat, weight, pheromon, q);
        }
        int best = -1;
        for (int i = 0; i < N; i++)
            if (ants[i].Lk < best_l)
            {
                best = i;
                best_l = ants[i].Lk;
            }
        if (best != -1)
            copy_array(route, ants[best].route);

        recalc_pheromon(&pheromon, &d_pheromon, p);
        recalc_weight(&weight, pheromon, visib, a, b);
    }

    solution_t solv = {best_l, route};
    free_ant_array(&ants, N);
    free_matrix(&pheromon);
    free_matrix(&weight);
    free_matrix(&visib);
    free_matrix(&d_pheromon);
    return solv;
}
