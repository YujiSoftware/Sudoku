LANG := c go java csharp rust ruby python

run:
	# Cache
	cat sudoku.csv > /dev/null
	# Run	
	@for lang in $(LANG); do \
		make -C $$lang run; \
    	done

version:
	@for lang in $(LANG); do \
		make -C $$lang version; \
	done
