SRC     := *.go
PROGRAM := sudoku
ARGS    := ../sudoku.csv 1000000
LANG    := go
TIME    := \time -f '$(LANG)\t%e'

run: $(PROGRAM)
	$(TIME) ./$(PROGRAM) $(ARGS)

$(PROGRAM): $(SRC)
	go build -o $(PROGRAM)

clean:; rm -f sudoku

version:; go version
