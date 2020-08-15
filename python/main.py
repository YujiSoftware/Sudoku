import sys

from solver import Solver
from timeit import default_timer as timer

def read(path, quizzes, solutions):
    with open(path, "rb") as f:
        f.readline()

        buf = bytearray(9 * 9)
        for i in range(len(quizzes)):
            f.readinto(buf)
            quiz = [0] * len(buf)
            for j in range(len(buf)):
                quiz[j] = buf[j] - ord("0")
            quizzes[i] = quiz

            f.read(1)

            f.readinto(buf)
            solution = [0] * len(buf)
            for j in range(len(buf)):
                solution[j] = buf[j] - ord("0")
            solutions[i] = solution

            f.read(1)

def solve(quizzes, solutions):
    for i in range(len(quizzes)):
        answer = Solver.solve(quizzes[i])
        
        if answer == None or not valid(answer, solutions[i]):
            raise AssertionError('Invalid answer. [index=%d]' % i)

        # export(answer)

def valid(answer, solution):
    for i in range(len(answer)):
        if answer[i] != solution[i]:
            return False
    return True

def export(answer):
    s = ""
    for c in answer:
        s += str(c)
    print(s)

path = sys.argv[1]
rows = int(sys.argv[2])

quizzes = [0] * rows
solutions = [0] * rows

print('Start read...', file=sys.stderr)
startRead = timer()
read(path, quizzes, solutions)
endRead = timer()

print('Start solve...', file=sys.stderr)
startSolve = timer()
solve(quizzes, solutions)
endSolve = timer()

readTime = (endRead - startRead) * 1000
solveTime = (endSolve - startSolve) * 1000
print('%fms\t%fms' % (readTime, solveTime), file=sys.stderr)
