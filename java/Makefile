pwd       := $(dir $(abspath $(firstword $(MAKEFILE_LIST))))
top_dir   := $(dir $(abspath $(pwd)))
java_home := $(top_dir)/runtime/jdk-14

run: javac
	$(java_home)/bin/java -classpath bin Main $(top_dir)/sudoku.csv 1000000

javac: bin/*.class

bin/*.class: src/*.java
	$(java_home)/bin/javac -d bin src/Main.java src/Solver.java

setup:
	mkdir -p $(top_dir)/runtime
	(cd $(top_dir)/runtime && curl -O -L https://github.com/AdoptOpenJDK/openjdk14-binaries/releases/download/jdk-14.0.2%2B12/OpenJDK14U-jdk_arm_linux_hotspot_14.0.2_12.tar.gz)
	(cd $(top_dir)/runtime && tar -xf OpenJDK14U-jdk_arm_linux_hotspot_14.0.2_12.tar.gz -C jdk-14)
