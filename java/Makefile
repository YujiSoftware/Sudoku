PROGRAM := bin/*.class
ARGS    := ../sudoku.csv 1000000

run: $(PROGRAM)
	java -classpath bin Main $(ARGS)

$(PROGRAM): src/*.java
	javac -d bin src/Main.java src/Solver.java

clean:; rm -rf bin

version:; java --version
