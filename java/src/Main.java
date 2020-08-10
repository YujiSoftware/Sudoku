import java.io.IOException;
import java.io.Reader;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.concurrent.TimeUnit;

public class Main {

	public static void main(String[] args) throws IOException {
		String file = args[0];
		int rows = Integer.parseInt(args[1]);

		byte[][] quizzes = new byte[rows][];
		byte[][] solutions = new byte[rows][];

		System.err.println("Start read...");
		long startRead = System.nanoTime();
		read(file, quizzes, solutions);
		long endRead = System.nanoTime();

		System.err.println("Start solve...");
		long startSolve = System.nanoTime();
		solve(quizzes, solutions);
		long endSolve = System.nanoTime();

		long readTime = TimeUnit.NANOSECONDS.toMillis(endRead - startRead);
		long solveTime = TimeUnit.NANOSECONDS.toMillis(endSolve - startSolve);
		System.err.println(readTime + "ms\t" + solveTime + "ms");

	}

	private static void read(String file, byte[][] quizzes, byte[][] solutions)
			throws IOException {
		try (Reader reader = Files.newBufferedReader(Paths.get(file))) {
			// 1行目をスキップ
			while (reader.read() != '\n')
				;

			char[] buf = new char[9 * 9];
			for (int i = 0; i < quizzes.length; i++) {
				// 問題
				reader.read(buf, 0, buf.length);
				byte[] quiz = new byte[buf.length];
				for (int j = 0; j < buf.length; j++) {
					quiz[j] = (byte) (buf[j] - '0');
				}
				quizzes[i] = quiz;

				// カンマ (読み捨て)
				reader.read();

				// 回答
				reader.read(buf, 0, buf.length);
				byte[] solution = new byte[buf.length];
				for (int j = 0; j < buf.length; j++) {
					solution[j] = (byte) (buf[j] - '0');
				}
				solutions[i] = solution;

				// 改行 (読み捨て)
				reader.read();
			}
		}
	}

	private static void solve(byte[][] quizzes, byte[][] solutions) {
		for (int i = 0; i < quizzes.length; i++) {
			byte[] answer = Solver.solve(quizzes[i]);

			if (answer == null || !valid(answer, solutions[i])) {
				throw new RuntimeException("Invalid answer. [index=" + i + "]");
			}

			// export(answer);
		}
	}

	private static boolean valid(byte[] answer, byte[] solution) {
		for (int i = 0; i < answer.length; i++) {
			if (answer[i] != solution[i]) {
				return false;
			}
		}
		return true;
	}

	private static void export(byte[] answer) {
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < answer.length; i++) {
			sb.append(answer[i]);
		}
		System.out.print(sb + "\n");
	}
}
