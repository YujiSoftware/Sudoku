use std::io::BufRead;
use std::env;
use std::fs::File;
use std::io::BufReader;
use std::io;
use std::time::Instant;

use sudoku::solver;

fn main() {
    let args: Vec<String> = env::args().collect();

    let file = &args[1];
    let rows: usize = args[2].parse().unwrap();

    eprintln!("Start read...");
    let start_read = Instant::now();
    let (quizzes, solutions) = read(file, rows).expect("Can't read.");
    let read_time = start_read.elapsed();

    eprintln!("Start resolve...");
    let start_solve = Instant::now();
    solve(quizzes, solutions);
    let solve_time = start_solve.elapsed();

    eprintln!("{}ms\t{}ms", read_time.as_millis(), solve_time.as_millis());
}

fn read(file: &String, rows: usize) -> io::Result<(Box<[solver::Board]>, Box<[solver::Board]>)> {
    let mut quizzes: Vec<solver::Board> = Vec::with_capacity(rows);
    let mut solutions: Vec<solver::Board> = Vec::with_capacity(rows);

    let f = File::open(file)?;
    let mut reader = BufReader::new(f);

    // 1行目をスキップ
    let mut buffer = String::new();
    reader.read_line(&mut buffer)?;

    for _i in 0..rows {
        buffer = String::new();
        reader.read_line(&mut buffer)?;
        let mut line = buffer.chars();

        // 問題
        let mut quiz: solver::Board = [0; solver::N_BOARD];
        for j in 0..quiz.len() {
            quiz[j] = (line.next().unwrap() as u8) - b'0';
        }
        quizzes.push(quiz);

        // カンマ
        line.next();

        // 回答
        let mut solution: solver::Board = [0; solver::N_BOARD];
        for j in 0..solution.len() {
            solution[j] = (line.next().unwrap() as u8) - b'0';
        }
        solutions.push(solution);
    }

    return Ok((quizzes.into_boxed_slice(), solutions.into_boxed_slice()));
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
