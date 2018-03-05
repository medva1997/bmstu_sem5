#ifndef FUNCTIONS_H
#define FUNCTIONS_H

int compar_int(const void *a, const void *b);
int dec_compar_int(const void *a, const void *b);

typedef int(*compare_t)(const void *, const void *);

void my_qsort(void *arr, size_t count, size_t size, compare_t compare);
void my_bubble_sort(void *arr, size_t count, size_t size, compare_t compare);
void my_selection_sort(void *arr, size_t count, size_t size, compare_t compare);

int is_sorted(int *arr, size_t count);
unsigned long long int tick(void);

int RandInt(int n);
void randomize_arr(int *arr, size_t count);

#endif // FUNCTIONS_H
