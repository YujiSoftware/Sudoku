pub const N_BOARD: usize = 9 * 9;
pub type Board = [u8; N_BOARD];

pub fn solve(quiz: Board) -> Result<Board, String> {
    let mut s = Sudoku::new();
    for i in 0..quiz.len() {
        if quiz[i] != 0 {
            s.update(i, quiz[i]);
        }
    }

    return solve_internal(&mut s);
}

fn solve_internal(s: &mut Sudoku) -> Result<Board, String> {
    let mut updated = true;
    let mut solved = false;

    while updated {
        updated = false;
        solved = true;

        for i in 0..s.answer.len() {
            if s.answer[i] != 0 {
                continue;
            }
            solved = false;

            let candidate = s.get_candidate_bit(i);
            if candidate == 0 {
                return Err("Unresolved.".to_string());
            }

            let lowest = lowest_one_bit(candidate);
            if candidate == lowest {
                s.update(i, to_num(lowest));
                updated = true;
            }
        }
    }

    if solved {
        return Ok(s.answer);
    }

    let mut count = usize::MAX;
    let mut index = 0;
    for i in 0..s.answer.len() {
        if s.answer[i] != 0 {
            continue;
        }

        let c = bit_count(s.get_candidate_bit(i));
        if c < count {
            count = c;
            index = i;
        }
    }

    let candidate = s.get_candidate_bit(index);
    for i in 0..9 {
        let bit = candidate & (1 << i);
        if bit == 0 {
            continue;
        }

        let mut new_s = Sudoku {
            answer: s.answer,
            rows: s.rows,
            columns: s.columns,
            regions: s.regions,
        };
        new_s.update(index, i + 1);

        if let Ok(answer) = solve_internal(&mut new_s) {
            return Ok(answer);
        }
    }

    return Err("Unresolved.".to_string());
}

fn lowest_one_bit(i: u16) -> u16 {
    return i & (!i + 1);
}

fn to_num(bit: u16) -> u8 {
    // 対数関数で計算できるが、パターンが少ないので switch の方が速い
    return match bit {
        1 => 1,
        2 => 2,
        4 => 3,
        8 => 4,
        16 => 5,
        32 => 6,
        64 => 7,
        128 => 8,
        256 => 9,
        _ => panic!("Invalid bit."),
    };
}

fn bit_count(value: u16) -> usize {
    let mut sum = 0;
    for i in 0..9 {
        if (value & 1 << i) != 0 {
            sum += 1;
        }
    }

    return sum;
}

struct Sudoku {
    answer: Board,
    rows: [u16; 9],
    columns: [u16; 9],
    regions: [u16; 9],
}

impl Sudoku {
    fn new() -> Sudoku {
        return Sudoku {
            answer: [0; N_BOARD],
            rows: [0; 9],
            columns: [0; 9],
            regions: [0; 9],
        };
    }

    fn update(&mut self, index: usize, number: u8) {
        self.answer[index] = number;

        let r: usize = index / 9;
        let c: usize = index % 9;
        let value: u16 = 1 << (number - 1);
        self.rows[r] |= value;
        self.columns[c] |= value;
        self.regions[(r / 3) * 3 + (c / 3)] |= value;
    }

    fn get_candidate_bit(&self, index: usize) -> u16 {
        let r: usize = index / 9;
        let c: usize = index % 9;

        let mut bit: u16 = 0;
        bit |= self.rows[r];
        bit |= self.columns[c];
        bit |= self.regions[(r / 3) * 3 + (c / 3)];

        return !bit & 0x1FF;
    }
}
