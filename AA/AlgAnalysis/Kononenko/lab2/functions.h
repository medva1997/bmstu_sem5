#ifndef FUNCTIONS_H
#define FUNCTIONS_H

typedef struct matrix_t
{
    int **matr;
    int n;
    int m;
}matrix_t;

unsigned long long int tick(void);
int RandInt(int n);
matrix_t create_matrix(const size_t n, const size_t m);
matrix_t create_random_matrix(const size_t n, const size_t m);

void print_matr(const matrix_t matrix);
void free_matrix(matrix_t *matrix);

matrix_t matr_mult(const matrix_t m1, const matrix_t m2, unsigned long long int *t);
matrix_t vinograd_mult(const matrix_t m1, const matrix_t m2, unsigned long long int *t);
matrix_t vinograd_mult_o2(const matrix_t m1, const matrix_t m2, unsigned long long int *t);

#endif // FUNCTIONS_H
