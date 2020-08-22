using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = args[0];
            var rows = int.Parse(args[1]);

            var quizzes = new byte[rows][];
            var solutions = new byte[rows][];

            Console.Error.WriteLine("Start read...");
            var readTime = Stopwatch.StartNew();
            Read(file, quizzes, solutions);
            readTime.Stop();

            Console.Error.WriteLine("Start solve...");
            var solveTime = Stopwatch.StartNew();
            Solve(quizzes, solutions);
            solveTime.Stop();

            Console.Error.WriteLine($"{readTime.ElapsedMilliseconds}ms\t{solveTime.ElapsedMilliseconds}ms");
        }

        private static void Read(string file, byte[][] quizzes, byte[][] solutions)
        {
            using (var reader = new StreamReader(file))
            {
                // 1行目をスキップ
                reader.ReadLine();

                for (int i = 0; i < quizzes.Length; i++)
                {
                    char[] line = reader.ReadLine().ToCharArray();
                    int index = 0;

                    // 問題
                    var quiz = new byte[81];
                    for (int j = 0; j < quiz.Length; j++)
                    {
                        quiz[j] = (byte)(line[index++] - '0');
                    }
                    quizzes[i] = quiz;

                    // カンマ
                    index++;

                    // 回答
                    var solution = new byte[81];
                    for (int j = 0; j < solution.Length; j++)
                    {
                        solution[j] = (byte)(line[index++] - '0');
                    }
                    solutions[i] = solution;
                }

            }
        }

        private static void Solve(byte[][] quizzes, byte[][] solutions)
        {
            for (int i = 0; i < quizzes.Length; i++)
            {
                var answer = Solver.Solve(quizzes[i]);

                if (answer == null || !Valid(answer, solutions[i]))
                {
                    throw new Exception($"Invalid answer. [index={i}]");
                }

                // Export(answer);
            }
        }

        private static bool Valid(byte[] answer, byte[] solution)
        {
            for (int i = 0; i < answer.Length; i++)
            {
                if (answer[i] != solution[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static void Export(byte[] answer)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < answer.Length; i++)
            {
                sb.Append(answer[i]);
            }
            Console.Write(sb + "\n");
        }
    }
}
