using System;
using System.Linq;

namespace Sudoku
{
    class Solver
    {
        public static byte[] Solve(byte[] quiz)
        {
            var sudoku = new Sudoku();
            for (int i = 0; i < quiz.Length; i++)
            {
                if (quiz[i] != 0)
                {
                    sudoku.Update(i, quiz[i]);
                }
            }

            return Solve(sudoku);
        }

        private static byte[] Solve(Sudoku sudoku)
        {
            bool updated;
            bool solved;
            do
            {
                updated = false;
                solved = true;

                for (int i = 0; i < 9 * 9; i++)
                {
                    if (sudoku.Answer[i] != 0)
                    {
                        continue;
                    }
                    solved = false;

                    int c = sudoku.GetCandidateBit(i);
                    if (c == 0)
                    {
                        return null;
                    }

                    int lowest = LowestOneBit(c);
                    if (c == lowest)
                    {
                        sudoku.Update(i, ToNum(lowest));
                        updated = true;
                    }
                }
            } while (updated);

            if (solved)
            {
                return sudoku.Answer;
            }

            // 候補が少ない箇所を探す
            int count = int.MaxValue;
            int index = 0;
            for (int i = 0; i < sudoku.Answer.Length; i++)
            {
                if (sudoku.Answer[i] != 0)
                {
                    continue;
                }

                int c = BitCount(sudoku.GetCandidateBit(i));
                if (c < count)
                {
                    count = c;
                    index = i;
                }
            }

            // 複数ある候補のうちの一つを仮定して、再帰的に解析
            int candidate = sudoku.GetCandidateBit(index);
            for (int i = 0; i < 9; i++)
            {
                int bit = (candidate & (1 << i));
                if (bit == 0)
                {
                    continue;
                }

                Sudoku newSudoku = new Sudoku(sudoku);
                newSudoku.Update(index, (byte)(i + 1));

                byte[] answer = Solve(newSudoku);
                if (answer != null)
                {
                    return answer;
                }
            }

            return null;
        }


        private static int BitCount(int value)
        {
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                if ((value & 1 << i) != 0)
                {
                    sum++;
                }
            }

            return sum;
        }

        private static int LowestOneBit(int i)
        {
            return i & -i;
        }


        private static byte ToNum(int bit)
        {
            // 対数関数で計算できるが、パターンが少ないので switch の方が速い
            switch (bit)
            {
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 4:
                    return 3;
                case 8:
                    return 4;
                case 16:
                    return 5;
                case 32:
                    return 6;
                case 64:
                    return 7;
                case 128:
                    return 8;
                case 256:
                    return 9;
                default:
                    throw new Exception("Invalid bit. [" + bit + "]");
            }
        }

        private class Sudoku
        {
            public byte[] Answer { get; }
            public short[] Rows { get; }
            public short[] Columns { get; }
            public short[] Regions { get; }

            public Sudoku()
            {
                this.Answer = new byte[9 * 9];
                this.Rows = new short[9];
                this.Columns = new short[9];
                this.Regions = new short[9];
            }

            public Sudoku(Sudoku other)
            {
                this.Answer = other.Answer.ToArray();
                this.Rows = other.Rows.ToArray();
                this.Columns = other.Columns.ToArray();
                this.Regions = other.Regions.ToArray();
            }

            public void Update(int index, byte number)
            {
                Answer[index] = number;

                int r = index / 9;
                int c = index % 9;
                short value = (short)(1 << (number - 1));
                Rows[r] |= value;
                Columns[c] |= value;
                Regions[(r / 3) * 3 + (c / 3)] |= value;
            }

            public int GetCandidateBit(int index)
            {
                int r = index / 9;
                int c = index % 9;

                short bit = 0;
                bit |= Rows[r];
                bit |= Columns[c];
                bit |= Regions[(r / 3) * 3 + (c / 3)];

                return ~bit & 0x1FF;
            }
        }
    }
}
