pwd       := $(dir $(abspath $(firstword $(MAKEFILE_LIST))))
top_dir   := $(dir $(abspath $(pwd)))
go_home   := $(top_dir)/runtime/go
PROGRAM   := sudoku

run: $(PROGRAM)
	./sudoku $(top_dir)/sudoku.csv 1000000

$(PROGRAM): main.go solver.go
	$(go_home)/bin/go build -o $(PROGRAM)

setup:
	mkdir -p $(top_dir)/runtime
	(cd $(top_dir)/runtime && curl -O -L "https://golang.org/dl/go1.15.linux-armv6l.tar.gz")
	(cd $(top_dir)/runtime && tar -xf go1.15.linux-armv6l.tar.gz)
