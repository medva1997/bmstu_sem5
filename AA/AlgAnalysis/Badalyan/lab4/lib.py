import random
from time import *

def base(m1, m2):
    m = []
    a, b, c = len(m1), len(m2), len(m2[0])
    if b != len(m1[0]):
        print("Error")
        return
    for i in range(a):
        m.append([0 for j in range(c)])
    for i in range(a):
        for j in range(c):
            for k in range(b):
                m[i][j] += m1[i][k]*m2[k][j]
    return m

def vinograd(m1, m2):
    a, b, c = len(m1), len(m2), len(m2[0])
    if b != len(m1[0]):
        print("Error")
        return
    d = b // 2
    row_factor = [0 for i in range(a)]
    col_factor = [0 for i in range(c)]

    for i in range(a):
        for j in range(d):
            row_factor[i] = row_factor[i] + m1[i][2 * j] * m1[i][2 * j + 1]

    for i in range(c):
        for j in range(d):
            col_factor[i] = col_factor[i] + m2[2 * j][i] * m2[2 * j + 1][i]

    m = [[0 for i in range(c)] for j in range(a)]

    for i in range(a):
        for j in range(c):
            m[i][j] = - row_factor[i] - col_factor[j]
            for k in range(d):
                m[i][j] = m[i][j] + ((m1[i][2 * k] + m2[2 * k + 1][j]) * (m1[i][2 * k + 1] + m2[2 * k][j]))
    if b % 2:
        for i in range(a):
            for j in range(c):
                m[i][j] = m[i][j] + m1[i][b - 1] * m2[b - 1][j]
    return m

def vinograd_mod(m1, m2):
    a, b, c = len(m1), len(m2), len(m2[0])
    if b != len(m1[0]):
        print("Error")
        return
    flag = (b % 2 == 1)
    row_factor = [0 for i in range(a)]
    col_factor = [0 for i in range(c)]

    for i in range(a):
        for j in range(1, b, 2):
            row_factor[i] -= m1[i][j - 1] * m1[i][j]

    for i in range(c):
        for j in range(1, b, 2):
            col_factor[i] -= m2[j - 1][i] * m2[j][i]

    m = [[0 for i in range(c)] for j in range(a)]

    for i in range(a):
        for j in range(c):
            m[i][j] += row_factor[i] + col_factor[j]
            for k in range(1, b, 2):
                m[i][j] += ((m1[i][k - 1] + m2[k][j]) * (m1[i][k] + m2[k - 1][j]))
                if flag:
                    m[i][j] += m1[i][b - 1] * m2[b - 1][j]
    return m

def test_time(f, len_mas, flag):
    t = 0
    m1 = [[int(random.uniform(-100, 100)) for i in range(len_mas)] for j in range(len_mas)]
    m2 = [[int(random.uniform(-100, 100)) for i in range(len_mas)] for j in range(len_mas)]
    t1 = clock()
    m = f(m1, m2)
    t2 = clock()
    t += t2 - t1
    m1 = [[int(random.uniform(-100, 100)) for i in range(len_mas)] for j in range(len_mas)]
    m2 = [[int(random.uniform(-100, 100)) for i in range(len_mas)] for j in range(len_mas)]
    t1 = clock()
    m = f(m1, m2)
    t2 = clock()
    t += t2 - t1
    m1 = [[int(random.uniform(-100, 100)) for i in range(len_mas)] for j in range(len_mas)]
    m2 = [[int(random.uniform(-100, 100)) for i in range(len_mas)] for j in range(len_mas)]
    t1 = clock()
    m = f(m1, m2)
    t2 = clock()
    t += t2 - t1
    m1 = [[int(random.uniform(-100, 100)) for i in range(len_mas)] for j in range(len_mas)]
    m2 = [[int(random.uniform(-100, 100)) for i in range(len_mas)] for j in range(len_mas)]
    t1 = clock()
    m = f(m1, m2)
    t2 = clock()
    t += t2 - t1
    m1 = [[int(random.uniform(-100, 100)) for i in range(len_mas)] for j in range(len_mas)]
    m2 = [[int(random.uniform(-100, 100)) for i in range(len_mas)] for j in range(len_mas)]
    t1 = clock()
    m = f(m1, m2)
    t2 = clock()
    t += t2 - t1
    t /= 5
    if flag == 1:
        print("Len = ", len_mas, "Base time: ", t*1000)
    else:
        if flag == 2:
            print("Len = ", len_mas, "Vino time: ", t*1000)
        else:
            print("Len = ", len_mas, "ModV time: ", t*1000)
    if flag == 3:
        print()

    return t
len_mas = 100
m1 = [[int(random.uniform(-100, 100)) for i in range(len_mas)] for j in range(len_mas)]
m2 = [[int(random.uniform(-100, 100)) for i in range(len_mas)] for j in range(len_mas)]
t1 = time()
m = vinograd(m1, m2)
t2 = time()
print(t2 - t1)