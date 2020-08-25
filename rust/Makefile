SRC     := src/*.rs
PROGRAM := target/release/sudoku
ARGS    := ../sudoku.csv 1000000

run: $(PROGRAM)
	./$(PROGRAM) $(ARGS)

$(PROGRAM): $(SRC)
	cargo build --release

clean:;	rm -rf target
version:; cargo version

