from base import *
from base_with_rec import *
from modified import *
from time import *
import random

##s1 = str(input("Введите первую строку: "))
##s2 = str(input("Введите вторую строку: "))
##
##dist = base_lev(s1,s2)
##print("Расстояние Левенштейна по базовуму алгоритму: ")
##print(dist)
##dist = modified_lev(s1,s2)
##print("Расстояние Левенштейна по модифицированному алгоритму: ")
##print(dist)
##dist = rec_lev(s1,s2)
##print("Расстояние Левенштейна по базовуму алгоритму c рекурсией: ")
##print(dist)

a = ["класс", " ", "клас", "кас", "гол", "иф"]
t1 = time()
for i in range(1, len(a) - 1):
    print(a[0], " ", a[i], " ", base_lev(a[0],a[i]), modified_lev(a[0],a[i]),\
          rec_lev(a[0],a[i]), "\n")

a = ["телефон", " ", "теле", "мышка", "гол", "собака"]

for i in range(1, len(a) - 1):
    print(a[0], " ", a[i], " ", base_lev(a[0],a[i]), modified_lev(a[0],a[i]),\
          rec_lev(a[0],a[i]), "\n")

a = []
b = ['a', 'b', 'c', 'd']
for i in range(1000):
    s = " "
    for j in range(0, int(random.uniform(0, 6))):
        s += b[int(random.uniform(0, 3))]
    a.append(s)

def test_time_base(mas, word):
    t1 = time()
    for i in range(len(mas)):
        base_lev(word, mas[i])
    t2 = time()
    return t2 - t1

def test_time_modi(mas, word):
    t1 = time()
    for i in range(len(mas)):
        modified_lev(word, mas[i])
    t2 = time()
    return t2 - t1

def test_time_rec(mas, word):
    t1 = time()
    for i in range(len(mas)):
        rec_lev(word, mas[i])
    t2 = time()
    return t2 - t1

t1_1 = test_time_base(a, "basdjfgh")
#print(t1_1)
t1_2 = test_time_base(a, "kdofookfdfd")
#print(t1_2)
t1_3 = test_time_base(a, "dfdfgrfgrrgdh")
#print(t1_3)
t1_4 = test_time_base(a, "basdjffghfghgh")
#print(t1_1)
t1_5 = test_time_base(a, "kdofookfgfgfggsfd")
#print(t1_2)
t1_6 = test_time_base(a, "dfdfgrfgrsfsdfrgdh")
#print(t1_3)
t1 = (t1_3 + t1_2 + t1_1 + t1_4 + t1_5 + t1_6)/6
print(t1)

t2_1 = test_time_modi(a, "basdjfgh")
#print(t2_1)
t2_2 = test_time_modi(a, "kdofookfdfd")
#print(t2_2)
t2_3 = test_time_modi(a, "dfdfgrfgrrgdh")
#print(t2_3)
t2_4 = test_time_modi(a, "basdjffghfghgh")
#print(t2_1)
t2_5 = test_time_modi(a, "kdofookfgfgfggsfd")
#print(t2_2)
t2_6 = test_time_modi(a, "dfdfgrfgrsfsdfrgdh")
#print(t2_3)
t2 = (t2_3 + t2_2 + t2_1 + t2_4 + t2_5 + t2_6)/6
print(t2)

t3_1 = test_time_rec(a, "basdjfgh")
#print(t3_1)
t3_2 = test_time_rec(a, "kdofookfdfd")
#print(t3_2)
t3_3 = test_time_rec(a, "dfdfgrfgrrgdh")
#print(t3_3)
t3_4 = test_time_rec(a, "basdjffghfghgh")
#print(t3_1)
t3_5 = test_time_rec(a, "kdofookfgfgfggsfd")
#print(t3_2)
t3_6 = test_time_rec(a, "dfdfgrfgrsfsdfrgdh")
#print(t3_3)
t3 = (t3_3 + t3_2 + t3_1 + t3_4 + t3_5 + t3_6)/6
print(t3)
