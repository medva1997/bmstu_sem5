#ifndef FUNCTIONS_H
#define FUNCTIONS_H

typedef struct rotor_t
{
     int permutations[26];
     int start_pos;
     int cur_pos;
}rotor_t;

typedef struct reflector_t
{
    int permutations[26];
}reflector_t;

rotor_t rotor_I;
rotor_t rotor_II;
rotor_t rotor_III;
reflector_t reflector_B;

void encryption(void);
void decrypting(void);

#endif // FUNCTIONS_H
