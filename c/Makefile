CC      := gcc
CFLAGS  := -Wall -O4
OBJS    := main.o solver.o
PROGRAM := sudoku
ARGS    := ../sudoku.csv 1000000

run: $(PROGRAM)
	./$(PROGRAM) $(ARGS)

$(PROGRAM): $(OBJS)
	$(CC) $(OBJS) $(CFLAGS) -o $(PROGRAM)

clean:;	rm -f *.o *~ $(PROGRAM)
version:; gcc --version

