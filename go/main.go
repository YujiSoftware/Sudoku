package main

import (
	"bufio"
	"fmt"
	"io"
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
	for {
		b, err := reader.ReadByte()
		if err != nil {
			panic(err)
		}
		if b == '\n' {
			break
		}
	}

	buf := make([]byte, 81)
	for i := 0; i < rows; i++ {
		// 問題
		if _, err := io.ReadFull(reader, buf); err != nil {
			panic(err)
		}
		quiz := board{}
		for j := 0; j < len(buf); j++ {
			num := buf[j] - '0'
			quiz[j] = uint8(num)
		}
		quizzes[i] = quiz

		// カンマ (読み捨て)
		if _, err := reader.ReadByte(); err != nil {
			panic(err)
		}

		// 回答
		if _, err := io.ReadFull(reader, buf); err != nil {
			panic(err)
		}
		solution := board{}
		for j := 0; j < len(solution); j++ {
			num := buf[j] - '0'
			solution[j] = uint8(num)
		}
		solutions[i] = solution

		// 改行 (読み捨て)
		if _, err := reader.ReadByte(); err != nil {
			panic(err)
		}
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
