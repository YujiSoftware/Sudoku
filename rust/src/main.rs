use std::env;
use std::fs::File;
use std::io::Read;
use std::time::Instant;

use sudoku::solver;

fn main() {
    let args: Vec<String> = env::args().collect();

    let file = &args[1];
    let rows: usize = args[2].parse().unwrap();

    eprintln!("Start read...");
    let start_read = Instant::now();
    let (quizzes, solutions) = read(file, rows);
    let read_time = start_read.elapsed();

    eprintln!("Start resolve...");
    let start_solve = Instant::now();
    solve(quizzes, solutions);
    let solve_time = start_solve.elapsed();

    eprintln!("{}ms\t{}ms", read_time.as_millis(), solve_time.as_millis());
}

fn read(file: &String, rows: usize) -> (Box<[solver::Board]>, Box<[solver::Board]>) {
    let mut quizzes: Vec<solver::Board> = Vec::with_capacity(rows);
    let mut solutions: Vec<solver::Board> = Vec::with_capacity(rows);

    let mut f = File::open(file).expect("file not found");

    // 1行目をスキップ
    let mut one = [0];
    while let Ok(_) = f.read(&mut one) {
        if one[0] as char == '\n' {
            break;
        }
    }

    let mut buf = [0; 81];
    for _i in 0..rows {
        // 問題
        f.read(&mut buf).expect("Can't read.");
        let mut quiz: solver::Board = [0; solver::N_BOARD];
        for j in 0..quiz.len() {
            let num = buf[j] - '0' as u8;
            quiz[j] = num;
        }
        quizzes.push(quiz);

        // カンマ（読み捨て）
        f.read(&mut one).expect("Can't read.");

        // 回答
        f.read(&mut buf).expect("Can't read.");
        let mut solution: solver::Board = [0; solver::N_BOARD];
        for j in 0..solution.len() {
            let num = buf[j] - '0' as u8;
            solution[j] = num;
        }
        solutions.push(solution);

        // 改行（読み捨て）
        f.read(&mut one).expect("Can't read.");
    }

    return (quizzes.into_boxed_slice(), solutions.into_boxed_slice());
}

fn solve(quizzes: Box<[solver::Board]>, solutions: Box<[solver::Board]>) {
    for i in 0..quizzes.len() {
        let answer = solver::solve(quizzes[i]);

        if answer.is_err() {
            panic!("Invalid answer. [err={}]", answer.err().unwrap());
        }

        let a = answer.unwrap();
        if !valid(a, solutions[i]) {
            panic!("Invalid answer. [index={}]", i);
        }

        // _export(a);
    }
}

fn valid(answer: solver::Board, solution: solver::Board) -> bool {
    for i in 0..answer.len() {
        if answer[i] != solution[i] {
            return false;
        }
    }

    return true;
}

fn _export(answer: solver::Board) {
    let mut s = "".to_string();
    for c in answer.iter() {
        s.push_str(&c.to_string());
    }

    println!("{}", s)
}
