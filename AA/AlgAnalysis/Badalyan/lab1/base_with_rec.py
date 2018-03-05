def rec_lev(s1, s2):
    l1, l2 = len(s1), len(s2)
    if l1 == 1 and l2 == 1:  # if s1 and s2 is symbols
        if s1 == s2:  # and they match
            return 0
        else:
            return 1
    else:
        if (l1 > l2 == 1) or (l2 > l1 == 1):
            return abs(l1 - l2) + 1

    t = 0
    if s1[-1] != s2[-1]:
        t = 1

    return min(rec_lev(s1[:l1 - 1], s2) + 1,
               rec_lev(s1, s2[:l2 - 1]) + 1,
               rec_lev(s1[:l1 - 1], s2[:l2 - 1]) + t)
