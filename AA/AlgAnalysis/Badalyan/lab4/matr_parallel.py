import concurrent.futures
from random import randint
from time import time


def get_str(m1, m2, i):
    b, c = len(m2), len(m2[0])
    row = [0 for j in range(c)]
    for j in range(c):
        for k in range(b):
            row[j] += m1[i][k] * m2[k][j]
    return row

def base(m1, m2):
    a, b = len(m1), len(m2)
    if b != len(m1[0]):
        print("Error")
        return
    #for i in range(a):
    #    m.append([0 for j in range(c)])
    m = []
    for i in range(a):
        m.append(get_str(m1, m2, i))

    return m

def base_parallel(m1, m2):
    a, b = len(m1), len(m2)
    if b != len(m1[0]):
        print("Error")
        return
    executor = concurrent.futures.ThreadPoolExecutor(max_workers=64)
    m = []
    for i in range(a):
        executor.submit(m.append, get_str(m1, m2, i))

    executor.shutdown()

    return m

def get_row_factor(m1, m2, i):
    d = len(m2) // 2
    res = 0
    for j in range(d):
        res = res + m1[i][2 * j] * m1[i][2 * j + 1]
    return res

def get_col_factor(m1, m2, i):
    d = len(m2) // 2
    res = 0
    for j in range(d):
        res = res + m2[2 * j][i] * m2[2 * j + 1][i]
    return res


def get_str_vinograd(m1, m2, i, row_factor, col_factor):
    b, c = len(m2), len(m2[0])
    d = len(m2) // 2
    row = [0 for j in range(c)]
    for j in range(c):
        row[j] = - row_factor[i] - col_factor[j]
        for k in range(d):
            row[j] = row[j] + ((m1[i][2 * k] + m2[2 * k + 1][j]) * (m1[i][2 * k + 1] + m2[2 * k][j]))

    return row

def get_str_vinograd_if_2(m1, m2, i, row):
    b = len(m2)
    for j in range(len(row)):
        row[j] = row[j] + m1[i][b - 1] * m2[b - 1][j]

    return row

def vinograd(m1, m2):
    a, b, c = len(m1), len(m2), len(m2[0])
    if b != len(m1[0]):
        print("Error")
        return
    d = b // 2
    row_factor = []
    col_factor = []

    for i in range(a):
        row_factor.append(get_row_factor(m1, m2, i))

    for i in range(c):
        col_factor.append(get_col_factor(m1, m2, i))

    m = []

    for i in range(a):
        m.append(get_str_vinograd(m1, m2, i, row_factor, col_factor))

    if b % 2:
        for i in range(a):
            m[i] = get_str_vinograd_if_2(m1, m2, i, m[i])

    return m

def vinograd_parallel(m1, m2):
    a, b, c = len(m1), len(m2), len(m2[0])
    if b != len(m1[0]):
        print("Error")
        return
    d = b // 2
    m = []
    row_factor = []
    col_factor = []
    executor = concurrent.futures.ThreadPoolExecutor(max_workers=64)
    for i in range(a):
        executor.submit(row_factor.append, get_row_factor(m1, m2, i))
    for i in range(c):
        executor.submit(col_factor.append, get_col_factor(m1, m2, i))

    for i in range(a):
        executor.submit(m.append, get_str_vinograd(m1, m2, i, row_factor, col_factor))

    executor.shutdown()

    if b % 2:
        for i in range(a):
            m[i] = get_str_vinograd_if_2(m1, m2, i, m[i])

    return m

def random_matrix(n, m):
    return [[randint(0, 10) for i in range(m)] for j in range(n)]

A = random_matrix(3, 3)
B = random_matrix(3, 3)
print(vinograd_parallel(A, B))
print(vinograd(A, B))
print(vinograd_parallel(A, B) == base(A, B))
