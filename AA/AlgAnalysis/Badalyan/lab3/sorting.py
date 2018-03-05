import random
from time import *
mas = [0, -1, 5, -2, 3]

def sheyker(mas):
    left = 0
    right = len(mas) - 1
    
    while left <= right:
        for i in range(left, right, +1):
            if mas[i] > mas[i + 1]:
                mas[i], mas[i + 1] = mas[i + 1], mas[i]
        right -= 1
    
        for i in range(right, left, -1):
            if mas[i - 1] > mas[i]:
                mas[i], mas[i - 1] = mas[i - 1], mas[i]
        left += 1

    return mas

#print(sheyker(mas))
mas = [0, -1, 5, -2, 3, -45]

def inserts(mas):
    for i in range(1, len(mas)):
        key = mas[i]
        j = i
        while j > 0 and mas[j-1] > key:
            mas[j] = mas[j-1]
            j -= 1
        mas[j] = key

    return mas

#print(inserts(mas))
mas = [0, -1, 5, -2, 3, -45]

def choise(mas):
    mas_len = len(mas)
    for i in range(mas_len):
        min = i
        for j in range(i + 1, mas_len):
            if mas[j] < mas[min]:
                min = j
        mas[min], mas[i] = mas[i], mas[min]

    return mas

def test_time(f, len_arr, f_time, flag, array, arrflag):
    t = 0
    t1 = f_time()
    f(array)
    t2 = f_time()
    t += t2 - t1
    if arrflag == 1:
        array = [random.uniform(-1000, 1000) for i in range(len_arr)]
    t1 = f_time()
    choise(array)
    t2 = f_time()
    t += t2 - t1
    if arrflag == 1:
        array = [random.uniform(-1000, 1000) for i in range(len_arr)]
    t1 = f_time()
    choise(array)
    t2 = f_time()
    t += t2 - t1
    if arrflag == 1:
        array = [random.uniform(-1000, 1000) for i in range(len_arr)]
    t1 = f_time()
    choise(array)
    t2 = f_time()
    t += t2 - t1
    if arrflag == 1:
        array = [random.uniform(-1000, 1000) for i in range(len_arr)]
    t1 = f_time()
    choise(array)
    t2 = f_time()
    t += t2 - t1

    if flag == 1:
        print("len = ", len_arr, "shey time = ", (t/5)*1000)
    else:
        if flag == 2:
            print("len = ", len_arr, "insr time = ", (t / 5)*1000)
        else:
            print("len = ", len_arr, "choi time = ", (t / 5)*1000)

    return t/5

len_arr = 500
arr1 = [i for i in range(len_arr)]
arr2 = [i for i in range(len_arr, -1, -1)]
arr3 = [random.uniform(-1000, 1000) for i in range(len_arr)]
arr4 = [1 for i in range(len_arr)]

test_time(sheyker, len_arr, clock, 1, arr4, 0)
test_time(inserts, len_arr, clock, 2, arr4, 0)
test_time(choise, len_arr, clock, 3, arr4, 0)