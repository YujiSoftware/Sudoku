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
                while (reader.Read() != '\n')
                    ;

                for (int i = 0; i < quizzes.Length; i++)
                {
                    var tmp = new char[9 * 9];

                    // 問題
                    reader.Read(tmp, 0, tmp.Length);
                    var quiz = new byte[tmp.Length];
                    for (int j = 0; j < tmp.Length; j++)
                    {
                        quiz[j] = (byte)(tmp[j] - '0');
                    }
                    quizzes[i] = quiz;

                    // カンマ (読み捨て)
                    reader.Read();

                    // 回答
                    reader.Read(tmp, 0, tmp.Length);
                    var solution = new byte[tmp.Length];
                    for (int j = 0; j < tmp.Length; j++)
                    {
                        solution[j] = (byte)(tmp[j] - '0');
                    }
                    solutions[i] = solution;

                    // 改行 (読み捨て)
                    reader.Read();
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
