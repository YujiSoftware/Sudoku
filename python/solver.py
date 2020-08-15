import sys

class Solver: 
    @classmethod
    def solve(cls, quiz):
        sudoku = Sudoku()
        for i in range(len(quiz)):
            if quiz[i] != 0:
                sudoku.update(i, quiz[i])
        return solve(sudoku)

def solve(sudoku):
    updated = True
    solved = False

    while updated:
        updated = False
        solved = True
        for i in range(len(sudoku.answer)):
            if sudoku.answer[i] != 0:
                continue
            solved = False

            candidate = sudoku.getCandidateBit(i)
            if candidate == 0:
                return None
            
            lowest = lowestOneBit(candidate)
            if candidate == lowest:
                sudoku.update(i, toNum(lowest))
                updated = True

    if solved:
        return sudoku.answer
    
    count = 9999
    index = 0
    for i in range(len(sudoku.answer)):
        if sudoku.answer[i] != 0:
            continue

        c = bitCount(sudoku.getCandidateBit(i))
        if c < count:
            count = c
            index = i

    candidate = sudoku.getCandidateBit(index)
    for i in range(9):
        bit = candidate & (1 << i)
        if bit == 0:
            continue

        newSudoku = Sudoku(sudoku)
        newSudoku.update(index, i + 1)
        
        answer = solve(newSudoku)
        if answer != None:
            return answer
    
    return None

def bitCount(value):
    sum = 0
    for i in range(9):
        if (value & (1 << i)) != 0:
            sum += 1

    return sum

def lowestOneBit(i):
    return i & -i

def toNum(bit):
    return {
        1: 1,
        2: 2,
        4: 3,
        8: 4,
        16: 5,
        32: 6,
        64: 7,
        128: 8,
        256: 9,
    }[bit]

class Sudoku:
    def __init__(self, other=None):
        if other == None :
            self.answer = [0] * (9 * 9)
            self.__rows = [0] * 9
            self.__columns = [0] * 9
            self.__regions = [0] * 9
        else:
            self.answer = other.answer[:]
            self.__rows = other.__rows[:]
            self.__columns = other.__columns[:]
            self.__regions = other.__regions[:]

    def update(self, index, number):
        self.answer[index] = number

        r = index // 9
        c = index % 9
        value = 1 << (number - 1)
        self.__rows[r] |= value
        self.__columns[c] |= value
        self.__regions[(r // 3) * 3 + (c // 3)] |= value

    def getCandidateBit(self, index):
        r = index // 9
        c = index % 9

        bit = 0
        bit |= self.__rows[r]
        bit |= self.__columns[c]
        bit |= self.__regions[(r // 3) * 3 + (c // 3)]

        return ~bit & 0x1FF
