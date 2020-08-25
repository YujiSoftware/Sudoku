PROGRAM := bin/Release/netcoreapp3.1/Sudoku
ARGS    := ../sudoku.csv 1000000

run: $(PROGRAM)
	 $(PROGRAM) $(ARGS)

$(PROGRAM): *.cs
	dotnet build --configuration Release

clean:; rm -rf bin

version:; dotnet --version

