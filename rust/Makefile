SRC     := src/*.rs
PROGRAM := target/release/sudoku
ARGS    := ../sudoku.csv 1000000
LANG    := rust
TIME    := \time -f '$(LANG)\t%e'

run: $(PROGRAM)
	$(TIME) ./$(PROGRAM) $(ARGS)

$(PROGRAM): $(SRC)
	cargo build --release

clean:;	rm -rf target
version:; cargo version

