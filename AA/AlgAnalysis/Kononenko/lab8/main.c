#include <stdlib.h>
#include <stdio.h>
#include "functions.h"
#include <time.h>
#include <conio.h>

/*
 *A B C D E F G H I J K  L  M  N  O  P  Q  R  S  T  U  V  W  X  Y  Z
 *0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25
 */

rotor_t rotor_I   = {4,10,12,5,11,6,3,16,21,25,13,19,14,22,24,7,23,20,18,15,0,8,1,17,2,9, 17};
rotor_t rotor_II  = {0,9,3,10,18,8,17,20,23,1,11,7,22,19,12,2,16,6,25,13,15,24,5,21,14,4, 5};
rotor_t rotor_III = {1,3,5,7,9,11,2,15,17,19,23,21,25,13,24,4,8,22,6,0,10,12,20,18,16,14, 22};
reflector_t reflector_B = {24,17,20,7,16,18,11,3,15,23,13,6,14,10,12,8,4,1,5,25,2,22,21,9,0,19};

int main(int argc, char** argv)
{
    int answ = -1;
    while (answ != '0')
    {
        printf("*******Menu********\n"
               "1.Encryption\n"
               "2.Decrypting\n"
               "0.Exit\n");
        answ = getch();
        if (answ == '1')
        {
            encryption();
        }
        if (answ == '2')
        {
            decrypting();
        }
        printf("\n");
    }
    printf("Godbue\n");
    return 0;
}
