import sys

from solver import Solver
from timeit import default_timer as timer

def read(path, quizzes, solutions):
    with open(path, "rb") as f:
        f.readline()

        for i in range(len(quizzes)):
            line = f.readline()
            index = 0

            quiz = [0] * 81
            for j in range(len(quiz)):
                quiz[j] = line[index] - 48 # = ord("0")
                index += 1
            quizzes[i] = quiz

            index += 1

            solution = [0] * 81
            for j in range(len(solution)):
                solution[j] = line[index] - 48  # = ord("0")
                index += 1
            solutions[i] = solution

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
