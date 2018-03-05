FLAGS := -std=c99 -g3 -O3 -gdwarf-2 -ggdb  #-Werror -Wall
CC := gcc
RM := rm -f 

main.exe: main.o functions.o
	$(CC) -ggdb -pthread -o $@ $^

%.o: %.c *.h
	$(CC) $(FLAGS) -c $<

clean:
	$(RM) *.exe *.o