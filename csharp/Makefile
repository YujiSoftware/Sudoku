PROGRAM := bin/Release/netcoreapp3.1/Sudoku
ARGS    := ../sudoku.csv 1000000
LANG    := csharp
TIME    := \time -f '$(LANG)\t%e'

run: $(PROGRAM)
	 $(TIME) $(PROGRAM) $(ARGS)

$(PROGRAM): *.cs
	dotnet build --configuration Release

clean:; rm -rf bin

version:; dotnet --version

