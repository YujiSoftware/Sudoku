require 'benchmark'
require './solver'

def read(file, quizzes, solutions)
    File.open(file) do |f|
        while(f.getc != "\n") do
        end

        buf = "x" * 9 * 9
        for i in 0...quizzes.length do
            # 問題
            buf = f.read(buf.length, buf)

            quiz = Array.new(buf.length)
            for j in 0...buf.length do
                quiz[j] = buf[j].to_i
            end
            quizzes[i] = quiz

            # カンマ（読み捨て）
            f.getc

            # 回答
            buf = f.read(buf.length, buf)
            solution = Array.new(buf.length)
            for j in 0...buf.length do
                solution[j] = buf[j].to_i
            end
            solutions[i] = solution

            # 改行（読み捨て）
            f.getc
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
