#ifndef FUNCTIONS_H
#define FUNCTIONS_H

typedef struct matrix_t
{
    float **matr;
    int n;
    int m;
}matrix_t;

typedef struct array_t
{
    float *arr;
    size_t size;
}array_t;

typedef struct ant_t
{
    int start_city;
    int curr_city;
    int Lk;
    array_t route;
    array_t Jk;
}ant_t;

typedef struct solution_t
{
    int l;
    array_t route;
}solution_t;

int RandInt(int n);
array_t create_array(size_t size);
void free_array(array_t *a);
void print_array(FILE *out, const array_t a);

matrix_t create_matrix(const size_t n, const size_t m);
matrix_t create_random_matrix(const size_t n);

void print_matr(const matrix_t matrix);
void free_matrix(matrix_t *matrix);

solution_t solve(matrix_t adj_mat, float a, float b, float p, int q, int t_max);

#endif // FUNCTIONS_H
