def base_lev(s1, s2):
    len1, len2 = len(s1), len(s2)
    matrix = []
    matrix.append([i for i in range(len2+1)])
    for i in range(1, len1+1):
        matrix.append([0]*(len2+1))
        matrix[i][0] = i
    for i in range(1, len1+1):
        for j in range(1, len2+1):
            remove, adding, exchange = matrix[i][j-1]+1,\
                                       matrix[i - 1][j]+1, matrix[i-1][j-1]
            if s1[i-1] != s2[j-1]:
                exchange += 1
            matrix[i][j] = min(remove, adding, exchange)
    return matrix[i][j]
