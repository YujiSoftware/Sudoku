import java.io.BufferedReader;
import java.io.IOException;
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
		try (BufferedReader reader = Files.newBufferedReader(Paths.get(file))) {
			// 1行目をスキップ
			reader.readLine();

			for (int i = 0; i < quizzes.length; i++) {
				char[] line = reader.readLine().toCharArray();
				int index = 0;

				// 問題
				byte[] quiz = new byte[81];
				for (int j = 0; j < quiz.length; j++) {
					quiz[j] = (byte) (line[index++] - '0');
				}
				quizzes[i] = quiz;

				// カンマ
				index++;

				// 回答
				byte[] solution = new byte[81];
				for (int j = 0; j < solution.length; j++) {
					solution[j] = (byte) (line[index++] - '0');
				}
				solutions[i] = solution;
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
		for (byte b : answer) {
			sb.append(b);
		}
		System.out.print(sb + "\n");
	}
}
