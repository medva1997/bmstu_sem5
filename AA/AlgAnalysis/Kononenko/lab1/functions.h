#ifndef FUNCTIONS_H
#define FUNCRTIONS_H

unsigned long long int tick(void);

int read_string(char* out);

int Levenstein_simple(const char* const s1, const char* const s2, unsigned long long int *t);
int Levenstein_Damer(const char* const  s1, const char* const s2, unsigned long long int *t);
int Levenstein_r(const char * const s1, const char * const s2);

#endif
