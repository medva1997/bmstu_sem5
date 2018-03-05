import random as rnd

MAX_DIS = 10  # maximum distance
MIN_DIS = 1  # minimum distance


m = 5  # count of ants and cities
e = 2  # count of elite ants

a = 2
b = 1
Q = MIN_DIS * m
t_max = 500
p = 0.5

def get_desire_matr(m):
    res = []
    n = len(m)
    for i in range(n):
        temp = []
        for j in range(n):
            if m[i][j] == 0:
                temp.append(0)
            else:
                temp.append(1/m[i][j])
        res.append(temp)
    return res

def update_feramon(p, teta_e, teta_k, teta):
    res = []
    n = len(teta)
    for i in range(n):
        temp = []
        for j in range(n):
            temp.append((1 - p)*teta[i][j] + teta_k[i][j] + teta_e)
        res.append(temp)
    return res

def aco(m, e, d, t_max, alpha, beta, p, q):
    nue = get_desire_matr(d)
    teta = [[rnd.uniform(0,1) for i in range(m)] for j in range(m)]
    T_min = None
    L_min = None

    t = 0

    while t < t_max:
        teta_k = [[0 for i in range(m)] for j in range(m)]

        for k in range(m):
            Tk = [k]
            Lk = 0
            i = k

            while len(Tk) != m:
                J = [r for r in range(m)]
                for c in Tk:
                    J.remove(c)

                P = [0 for a in J]

                for j in J:
                    if d[i][j] != 0:
                        buf = sum((teta[i][l] ** alpha) * (nue[i][l] ** beta) for l in J)
                        P[J.index(j)] = (teta[i][j] ** alpha) * (nue[i][j] ** beta) / buf
                    else:
                        P[J.index(j)] = 0

                Pmax = max(P)
                if Pmax == 0:
                    break

                index = P.index(Pmax)
                Tk.append(J[index])
                Lk += d[i][J[index]]
                i = J.pop(index)

            if L_min is None or (Lk + d[Tk[0]][Tk[-1]]) < L_min:
                L_min = Lk + d[Tk[0]][Tk[-1]]
                T_min = Tk

            for g in range(len(Tk) - 1):
                a = Tk[g]
                b = Tk[g + 1]
                teta_k[a][b] += q / Lk

        teta_e = (e * q / L_min) if L_min else 0
        teta = update_feramon(p, teta_e, teta_k, teta)
        t += 1

    return T_min, L_min

test = [[0, 6, 3, 7, 1], \
        [6, 0, 8, 10, 6], \
        [3, 8, 0, 6, 7],\
        [7, 10, 6, 0, 5],\
        [1, 6, 7, 5, 0]]

print(aco(m, e, test, t_max, a, b, p, Q))