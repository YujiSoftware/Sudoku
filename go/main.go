package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
	"time"
)

type board [9 * 9]byte

func main() {
	args := os.Args
	file := args[1]
	rows, err := strconv.Atoi(args[2])
	if err != nil {
		panic(err)
	}

	fmt.Fprintln(os.Stderr, "Start read...")
	startRead := time.Now()
	quizzes, solutions := read(file, rows)
	endRead := time.Now()

	fmt.Fprintln(os.Stderr, "Start solve...")
	startSolve := time.Now()
	for i := 0; i < len(quizzes); i++ {
		answer, err := solve(quizzes[i])

		if err != nil || !valid(answer, solutions[i]) {
			panic(fmt.Sprintf("Invalid answer. [index=%d]", i))
		}

		// export(answer)
	}
	endSolve := time.Now()

	readTime := endRead.Sub(startRead).Milliseconds()
	solveTime := endSolve.Sub(startSolve).Milliseconds()
	fmt.Fprintf(os.Stderr, "%dms\t%dms\n", readTime, solveTime)
}

func read(file string, rows int) ([]board, []board) {
	quizzes := make([]board, rows)
	solutions := make([]board, rows)

	fp, err := os.Open(file)
	if err != nil {
		panic(err)
	}
	defer fp.Close()

	reader := bufio.NewReader(fp)

	// 1行目をスキップ
	if _, _, err := reader.ReadLine(); err != nil {
		panic(err)
	}

	line := make([]byte, 0, 81+1+81)
	for i := 0; i < rows; i++ {
		line = line[:0]
		isPrefix := true
		for isPrefix {
			var buf []byte
			buf, isPrefix, err = reader.ReadLine()
			if err != nil {
				panic(err)
			}

			line = append(line, buf...)
		}

		index := 0

		// 問題
		quiz := board{}
		for j := 0; j < len(quiz); j++ {
			quiz[j] = uint8(line[index] - '0')
			index++
		}
		quizzes[i] = quiz

		// カンマ
		index++

		// 回答
		solution := board{}
		for j := 0; j < len(solution); j++ {
			solution[j] = uint8(line[index] - '0')
			index++
		}
		solutions[i] = solution
	}

	return quizzes, solutions
}

func valid(answer, solution board) bool {
	for i := 0; i < len(answer); i++ {
		if answer[i] != solution[i] {
			return false
		}
	}
	return true
}

func export(answer board) {
	builder := strings.Builder{}
	for i := 0; i < len(answer); i++ {
		builder.WriteString(strconv.FormatUint(uint64(answer[i]), 10))
	}
	fmt.Println(builder.String())
}
