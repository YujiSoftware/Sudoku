CC      := gcc
CFLAGS  := -Wall -O4
OBJS    := main.o solver.o
PROGRAM := sudoku
ARGS    := ../sudoku.csv 1000000
LANG    := c
TIME    := \time -f '$(LANG)\t%e'

run: $(PROGRAM)
	$(TIME) ./$(PROGRAM) $(ARGS)

$(PROGRAM): $(OBJS)
	$(CC) $(OBJS) $(CFLAGS) -o $(PROGRAM)

clean:;	rm -f *.o *~ $(PROGRAM)
version:; gcc --version

