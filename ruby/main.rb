require 'benchmark'
require_relative 'solver'

def read(file, quizzes, solutions)
    File.open(file) do |f|
        f.readline

        for i in 0...quizzes.length do
            line = f.readline
            index = 0

            # 問題
            quiz = Array.new(81)
            for j in 0...quiz.length do
                quiz[j] = line[index].ord - 48  # = '0'
                index += 1
            end
            quizzes[i] = quiz

            # カンマ
            index += 1

            # 回答
            solution = Array.new(81)
            for j in 0...solution.length do
                solution[j] = line[index].ord - 48  # = '0'
                index += 1
            end
            solutions[i] = solution
        end
    end
end

def solve(quizzes, solutions)
    for i in 0...quizzes.length do
        answer = Solver.solve(quizzes[i])

        if answer == nil || !valid(answer, solutions[i]) then
            raise "Invalid answer. [index=#{i}]"
        end

        # export(answer)
    end
end

def valid(answer, solution)
    for i in 0...answer.length do
        if answer[i] != solution[i] then
            return false
        end
    end
    return true
end

def export(answer)
    for b in answer do
        print b
    end
    puts
end

file = ARGV[0]
rows = ARGV[1].to_i

quizzes = Array.new(rows)
solutions = Array.new(rows)

STDERR.puts "Start read..."
readTime = Benchmark.realtime do
    read(file, quizzes, solutions)
end

STDERR.puts "Start solve..."
solveTime = Benchmark.realtime do
    solve(quizzes, solutions)
end

STDERR.puts "#{(readTime * 1000).to_i}ms\t#{(solveTime * 1000).to_i}ms"
