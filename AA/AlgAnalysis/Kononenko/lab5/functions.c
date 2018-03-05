#include <stdlib.h>
#include <stdio.h>
#include "functions.h"
#include <time.h>
#include <pthread.h>
#include <math.h>
#include <windows.h>

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

node_t *create_node(const float val)
{
    node_t *node = malloc(sizeof(node_t));
    if (node == NULL)
    {
        fprintf(stderr, "ALLOCATION ERROR");
        return NULL;
    }
    node->value = val;
    node->next = NULL;
    return node;
}

queue_t create_queue(void)
{
    queue_t q= {NULL, NULL, 0};
    return q;
}

void push_q(queue_t *q, const float val)
{
    node_t *node = create_node(val);
    if (q->head == NULL)
    {
        q->head = node;
    }
    else
        q->tail->next = node;
    q->tail = node;
    (q->count)++;
}

static void push_q_node(queue_t *q, node_t *node)
{
    if (q->head == NULL)
    {
        q->head = node;
    }
    else
        q->tail->next = node;
    q->tail = node;
    (q->count)++;
}

void push_random_q(queue_t *q, size_t count)
{
    for (int i  = 0; i < count; i ++)
        push_q(q, RandInt(100));
}

float pop_q(queue_t *q)
{
    float res = 0;
    if (q->count > 0)
    {
        node_t *tmp = q->head;

        res = q->head->value;
        q->head = q->head->next;
        free(tmp);
        (q->count)--;
    }
    return res;
}

static node_t *pop_q_node(queue_t *q)
{
    node_t *tmp = NULL;
    if (q->count > 0)
    {
        tmp = q->head;
        q->head = q->head->next;
        (q->count)--;
    }
    return tmp;
}

void free_q(queue_t *q)
{
    node_t *tmp = q->head;
    while (q->head)
    {
        q->head = q->head->next;
        free(tmp);
        tmp = q->head;
    }
    q->tail = NULL;
    q->count = 0;
}

queue_arr create_queue_arr(size_t size)
{
    queue_arr q;
    q.data = malloc(sizeof(float) * size);
    q.count = 0;
    q.size = size;
    q.head = q.data;
    q.tail = q.data;
    return q;
}

void push_q_arr(queue_arr *q, const float val)
{
    *q->tail = val;
    q->tail++;
    q->count++;
}

float pop_q_arr(queue_arr *q)
{
    float res = 0;
    if (q->count > 0)
    {
        res = *q->head;
        q->head++;
        q->count--;
    }
    return res;
}

void push_random_q_arr(queue_arr *q, size_t count)
{
    for (int i  = 0; i < count; i ++)
        push_q_arr(q, RandInt(100));
}

void free_queue_arr(queue_arr *q)
{
    free(q->data);
    q->data = NULL;
    q->head = NULL;
    q->tail = NULL;
    q->count = 0;
    q->size = 0;
}


array_t get_random_array(size_t size)
{
    static int flag = 1;
    if (flag)
    {
        srand(time(NULL));
        flag = 0;
    }
    array_t a;
    a.arr = malloc(size * sizeof(float));
    a.size = size;
    for (int i = 0; i < size; i ++)
        a.arr[i] = RandInt(1000) * RandDbl();
    return a;
}

void free_array(array_t *arr)
{
    free(arr->arr);
    arr->arr = NULL;
    arr->size = 0;
}

static void f1(float *num)
{
    *num = pow(*num, 3);
}
static void f2(float *num)
{
    *num = sqrt(*num);
}
static void f3(float *num)
{
    *num = sin(*num);
}

queue_t process_data(queue_t *q, unsigned long long *t)
{
    *t = tick();
    queue_t res_q = create_queue();
    while (q->count)
    {
        node_t *num = pop_q_node(q);
        f1(&num->value);
        f2(&num->value);
        f3(&num->value);
        push_q_node(&res_q, num);
    }
    *t = tick() - *t;
    return res_q;
}
CRITICAL_SECTION cs1, cs2, cs3;
static void *conv1(void *args)
{
    convargs_t arg = *(convargs_t *)args;

    while (arg.todo->count || !*arg.pf)
    {
        while (arg.todo->count)
        {
            node_t *num = pop_q_node(arg.todo);

            f1(&num->value);

            EnterCriticalSection(&cs1);
            push_q_node(arg.next, num);
            LeaveCriticalSection(&cs1);
        }
    }
    *arg.nf = 1;
}

static void *conv2(void *args)
{
    convargs_t arg = *(convargs_t *)args;
    while (arg.todo->count || !*arg.pf)
    {
        while (arg.todo->count)
        {
            EnterCriticalSection(&cs1);
            node_t *num = pop_q_node(arg.todo);
            LeaveCriticalSection(&cs1);

            f2(&num->value);

            EnterCriticalSection(&cs2);
            push_q_node(arg.next, num);
            LeaveCriticalSection(&cs2);
        }
    }
    *arg.nf = 1;
}

static void *conv3(void *args)
{
    convargs_t arg = *(convargs_t *)args;
    while (arg.todo->count || !*arg.pf)
    {
        while (arg.todo->count)
        {
            EnterCriticalSection(&cs2);
            node_t *num = pop_q_node(arg.todo);
            LeaveCriticalSection(&cs2);

            f3(&num->value);

            push_q_node(arg.next, num);
        }
    }
    *arg.nf = 1;
}

queue_t process_data_par(queue_t *q, unsigned long long *t)
{
    InitializeCriticalSection(&cs1);
    InitializeCriticalSection(&cs2);
    *t = tick();
    pthread_t t1, t2, t3;
    queue_t q2 = create_queue(),
              q3 = create_queue(),
            res_q = create_queue();
    int f0 = 1, f1 = 0, f2 = 0, f3 = 0;
    convargs_t args1 = {q, &q2, &f0, &f1}, args2 = {&q2, &q3, &f1, &f2},
               args3 = {&q3, &res_q, &f2, &f3};
    ;
    pthread_create(&t1, NULL, conv1, &args1);
    pthread_create(&t2, NULL, conv2, &args2);
    pthread_create(&t3, NULL, conv3, &args3);

    pthread_join(t1, NULL);
    pthread_join(t2, NULL);
    pthread_join(t3, NULL);
    *t = tick() - *t;
    return res_q;
}
