#include <stdlib.h>
#include <stdio.h>
#include "functions.h"
#include <time.h>
#include <conio.h>

#define NOTHING 0

static int rotate_rot(rotor_t *rot)
{
    rot->cur_pos++;
    (rot->cur_pos > 25) ? (rot->cur_pos = 0) : NOTHING;
    return (rot->cur_pos == rot->start_pos) ? 1 : 0;
}

static void rotate_rotors(void)
{
    if (rotate_rot(&rotor_I))
        if(rotate_rot(&rotor_II))
            rotate_rot(&rotor_III);
}

static char dec26(char c, char symb)
{
    c -= symb;
    c < 0 ? c+=26 : NOTHING;
    return c;
}

static int ind_of(int perm[26], int num)
{
    int i = 0;
    while (perm[i++] != num);
    return --i;
}
//2 20 16
static char magic(char c)
{
    c -= 'a';
    c = (c + rotor_I.cur_pos) % 26;
    c = rotor_I.permutations[c];

    c = (c + dec26(rotor_II.cur_pos,rotor_I.cur_pos)) % 26;
    c = rotor_II.permutations[c];


    c = (c + dec26(rotor_III.cur_pos,rotor_II.cur_pos)) % 26;
    c = rotor_III.permutations[c];


    c = dec26(c, rotor_III.cur_pos);
    c = reflector_B.permutations[c];


    c = (c + rotor_III.cur_pos) % 26;
    c = ind_of(rotor_III.permutations, c);


    c = dec26(c,dec26(rotor_III.cur_pos,rotor_II.cur_pos));
    c = ind_of(rotor_II.permutations, c);


    c = dec26(c, dec26(rotor_II.cur_pos,rotor_I.cur_pos));
    c = ind_of(rotor_I.permutations, c);


    c = dec26(c, rotor_I.cur_pos);

    return c + 'a';
}

void encryption(void)
{
    printf("******Encription******\n"
           "Enter roters position(3 num[0,25] sep. by space : ");
    int p1, p2, p3;
    scanf("%d %d %d", &p1, &p2, &p3);
    rotor_I.cur_pos   = p3;
    rotor_II.cur_pos  = p2;
    rotor_III.cur_pos = p1;
    printf("Input your message here(/ to cancel) :\n");

    rotate_rotors();
    char c = getch();
    while (c != '/')
    {
        if (c >= 'a' && c <= 'z')
        {
            c = magic(c);
        }
        printf("%c", c);
        rotate_rotors();
        c = getch();
    }
    printf("\n");
}

void decrypting(void)
{
    printf("******decripting******\n"
           "Enter roters position(3 num[0,25] sep. by space : ");
    int p1, p2, p3;
    scanf("%d %d %d", &p1, &p2, &p3);
    rotor_I.cur_pos   = p3;
    rotor_II.cur_pos  = p2;
    rotor_III.cur_pos = p1;
    printf("Input your message here(/ to cancel) :\n");

    rotate_rotors();
    char c = getch();
    while (c != '/')
    {
        if (c >= 'a' && c <= 'z')
        {
            c = magic(c);
        }
        printf("%c", c);
        rotate_rotors();
        c = getch();
    }
    printf("\n");
}
