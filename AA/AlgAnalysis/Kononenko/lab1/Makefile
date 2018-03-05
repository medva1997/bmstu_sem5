FLAGS := -std=c99 -g3 -gdwarf-2 -ggdb #-Werror -Wall
CC := gcc
RM := rm -f 

main.exe: main.o functions.o
	$(CC) -ggdb -o $@ $^

%.o: %.c *.h
	$(CC) $(FLAGS) -c $<

clean:
	$(RM) *.exe *.o