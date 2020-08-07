package main

import (
	"fmt"
	"math"
	"math/bits"
)

type sudoku struct {
	answer  board
	rows    [9]uint16
	columns [9]uint16
	regions [9]uint16
}

func solve(quiz board) (board, error) {
	s := new(sudoku)
	for i := 0; i < len(quiz); i++ {
		if quiz[i] != 0 {
			s.update(i, quiz[i])
		}
	}

	return solveInternal(s)
}

func solveInternal(s *sudoku) (board, error) {
	updated := true
	solved := false

	for updated {
		updated = false
		solved = true

		for i := 0; i < len(s.answer); i++ {
			if s.answer[i] != 0 {
				continue
			}
			solved = false

			candidate := s.getCandidateBit(i)
			if candidate == 0 {
				return s.answer, fmt.Errorf("Unresolved")
			}

			highest := highestOneBit(candidate)
			lowest := lowestOneBit(candidate)
			if highest == lowest {
				s.update(i, toNum(int(lowest)))
				updated = true
			}
		}
	}

	if solved {
		return s.answer, nil
	}

	// 候補が少ない箇所を探す
	count := math.MaxInt32
	index := 0
	for i := 0; i < len(s.answer); i++ {
		if s.answer[i] != 0 {
			continue
		}

		c := bits.OnesCount16(s.getCandidateBit(i))
		if c < count {
			count = c
			index = i
		}
	}

	// 複数ある候補のうちの一つを仮定して、再帰的に解析
	candidate := s.getCandidateBit(index)
	for i := 0; i < 9; i++ {
		bit := candidate & (1 << i)
		if bit == 0 {
			continue
		}

		newS := &sudoku{
			answer:  s.answer,
			rows:    s.rows,
			columns: s.columns,
			regions: s.regions,
		}
		newS.update(index, toNum(int(bit)))

		if answer, err := solveInternal(newS); err == nil {
			return answer, nil
		}
	}

	return s.answer, fmt.Errorf("Unresolved")
}

func (s *sudoku) update(index int, number uint8) {
	s.answer[index] = number

	r := index / 9
	c := index % 9
	value := uint16(1 << (number - 1))
	s.rows[r] |= value
	s.columns[c] |= value
	s.regions[(r/3)*3+(c/3)] |= value
}

func (s *sudoku) getCandidateBit(index int) uint16 {
	r := index / 9
	c := index % 9

	bit := uint16(0)
	bit |= s.rows[r]
	bit |= s.columns[c]
	bit |= s.regions[(r/3)*3+(c/3)]

	return ^bit & 0x1FF
}

func highestOneBit(i uint16) uint16 {
	return i & (math.MaxUint16 >> bits.LeadingZeros16(i))
}

func lowestOneBit(i uint16) uint16 {
	return i & -i
}

func toNum(bit int) uint8 {
	// 対数関数で計算できるが、パターンが少ないので switch の方が速い
	switch bit {
	case 1:
		return 1
	case 2:
		return 2
	case 4:
		return 3
	case 8:
		return 4
	case 16:
		return 5
	case 32:
		return 6
	case 64:
		return 7
	case 128:
		return 8
	case 256:
		return 9
	default:
		panic("Invalid bit.")
	}
}
