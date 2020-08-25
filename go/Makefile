SRC     := *.go
PROGRAM := sudoku
ARGS    := ../sudoku.csv 1000000

run: $(PROGRAM)
	./$(PROGRAM) $(ARGS)

$(PROGRAM): $(SRC)
	go build -o $(PROGRAM)

clean:; rm -f sudoku

version:; go version
