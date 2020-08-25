DIRS := c go rust java csharp ruby python

run:
	@# Cache
	@cat sudoku.csv > /dev/null
	@# Run	
	@for dir in $(DIRS); do \
		make -C $$dir run; \
    	done

version:
	@for dir in $(DIRS); do \
		make -C $$dir version; \
	done
