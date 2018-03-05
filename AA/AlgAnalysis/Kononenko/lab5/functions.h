#ifndef FUNCTIONS_H
#define FUNCTIONS_H

typedef struct node_t
{
    float value;
    struct node_t *next;
}node_t;

typedef struct queue
{
    node_t *head;
    node_t *tail;
    size_t count;
}queue_t;

//one-off queue
typedef struct queue_arr
{
    float *data;
    float *head;
    float *tail;
    size_t size;
    size_t count;
}queue_arr;

typedef struct array_t
{
    float *arr;
    size_t size;
}array_t;

int RandInt(int n);

array_t get_random_array(size_t size);
void free_array(array_t *arr);

queue_t create_queue(void);
void push_q(queue_t *q, const float val);
float pop_q(queue_t *q);
void free_q(queue_t *q);
void push_random_q(queue_t *q, size_t count);

queue_arr create_queue_arr(size_t size);
void push_q_arr(queue_arr *q, const float val);
float pop_q_arr(queue_arr *q);
void push_random_q_arr(queue_arr *q, size_t count);
void free_queue_arr(queue_arr *q);

queue_t process_data(queue_t *q, unsigned long long *t);

typedef struct convargs_t
{
    queue_t *todo;
    queue_t *next;
    int *pf;
    int *nf;
}convargs_t;

typedef void *(*conv_t)(void *);
queue_t process_data_par(queue_t *q, unsigned long long *t);

#endif // FUNCTIONS_H
