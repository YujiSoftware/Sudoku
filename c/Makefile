pwd       := $(dir $(abspath $(firstword $(MAKEFILE_LIST))))
top_dir   := $(dir $(abspath $(pwd)))
CC = gcc
CFLAGS = -O4
OBJS = main.o solver.o
PROGRAM = sudoku

all: $(PROGRAM)

$(PROGRAM): $(OBJS)
	$(CC) $(OBJS) $(LDFLAGS) -o $(PROGRAM)

clean:;	rm -f *.o *~ $(PROGRAM)

run: $(PROGRAM)
	./$(PROGRAM) $(top_dir)/sudoku.csv 1000000

