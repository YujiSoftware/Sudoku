PROGRAM := bin/*.class
ARGS    := ../sudoku.csv 1000000
LANG    := java
TIME    := \time -f '$(LANG)\t%e'

run: $(PROGRAM)
	$(TIME) java -classpath bin Main $(ARGS)

$(PROGRAM): src/*.java
	javac -d bin src/Main.java src/Solver.java

clean:; rm -rf bin

version:; java --version
