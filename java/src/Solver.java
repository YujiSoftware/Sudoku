import java.util.Arrays;

public class Solver {

	public static byte[] solve(byte[] quiz) {
		Sudoku sudoku = new Sudoku();
		for (int i = 0; i < quiz.length; i++) {
			if (quiz[i] != 0) {
				sudoku.update(i, quiz[i]);
			}
		}
		return solve(sudoku);
	}

	private static byte[] solve(Sudoku sudoku) {
		boolean updated;
		boolean solved;
		do {
			updated = false;
			solved = true;

			for (int i = 0; i < 9 * 9; i++) {
				if (sudoku.answer[i] != 0) {
					continue;
				}
				solved = false;

				int candidate = sudoku.getCandidateBit(i);
				if (candidate == 0) {
					return null;
				}

				int lowest = lowestOneBit(candidate);
				if (candidate == lowest) {
					sudoku.update(i, toNum(lowest));
					updated = true;
				}
			}
		} while (updated);

		if (solved) {
			return sudoku.answer;
		}

		// 候補が少ない箇所を探す
		int count = Integer.MAX_VALUE;
		int index = 0;
		for (int i = 0; i < sudoku.answer.length; i++) {
			if (sudoku.answer[i] != 0) {
				continue;
			}

			int c = bitCount(sudoku.getCandidateBit(i));
			if (c < count) {
				count = c;
				index = i;
			}
		}

		// 複数ある候補のうちの一つを仮定して、再帰的に解析
		int candidate = sudoku.getCandidateBit(index);
		for (int i = 0; i < 9; i++) {
			int bit = candidate & 1 << i;
			if (bit == 0) {
				continue;
			}

			Sudoku newSudoku = new Sudoku(sudoku);
			newSudoku.update(index, (byte) (i + 1));

			byte[] answer = solve(newSudoku);
			if (answer != null) {
				return answer;
			}
		}

		return null;
	}

	private static int bitCount(int value) {
		int sum = 0;
		for (int i = 0; i < 9; i++) {
			if ((value & 1 << i) != 0) {
				sum++;
			}
		}

		return sum;
	}

	private static int lowestOneBit(int i) {
		return i & -i;
	}

	private static byte toNum(int bit) {
		// 対数関数で計算できるが、パターンが少ないので switch の方が速い
		switch (bit) {
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
			throw new RuntimeException("Invalid bit. [" + bit + "]");
		}
	}

	private static class Sudoku {
		private final byte[] answer;
		private final short[] rows;
		private final short[] columns;
		private final short[] regions;

		private Sudoku() {
			this.answer = new byte[9 * 9];
			this.rows = new short[9];
			this.columns = new short[9];
			this.regions = new short[9];
		}

		private Sudoku(Sudoku other) {
			this.answer = Arrays.copyOf(other.answer, other.answer.length);
			this.rows = Arrays.copyOf(other.rows, other.rows.length);
			this.columns = Arrays.copyOf(other.columns, other.columns.length);
			this.regions = Arrays.copyOf(other.regions, other.regions.length);
		}

		private void update(int index, byte number) {
			answer[index] = number;

			int r = index / 9;
			int c = index % 9;
			int value = (1 << (number - 1));
			rows[r] |= value;
			columns[c] |= value;
			regions[(r / 3) * 3 + (c / 3)] |= value;
		}

		private int getCandidateBit(int index) {
			int r = index / 9;
			int c = index % 9;

			int bit = 0;
			bit |= rows[r];
			bit |= columns[c];
			bit |= regions[(r / 3) * 3 + (c / 3)];

			return ~bit & 0x1FF;
		}
	}
}
