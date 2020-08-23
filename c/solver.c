#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <limits.h>
#include <string.h>

typedef struct {
	unsigned char answer[81];
	unsigned short rows[9];
	unsigned short columns[9];
	unsigned short regions[9];
} sudoku_t;

unsigned char* _solve_internal(sudoku_t *s);

void _update(sudoku_t *s, int index, unsigned char number);
int _get_candidate_bit(sudoku_t *s, int index);

int _bit_count(unsigned short value);
unsigned short _lowest_one_bit(unsigned short i);
unsigned char _to_num(int bit);

unsigned char* solve(unsigned char *quiz) {
    sudoku_t s;
    int i;

    memset(&s, 0, sizeof(sudoku_t));

    for(i = 0; i < 81; i++){
        if(quiz[i] != 0) {
            _update(&s, i, quiz[i]);
        }
    }

    return _solve_internal(&s);
}

unsigned char* _solve_internal(sudoku_t *s){
    bool updated, solved;
    int i;
    int candidate, lowest;
    int count, index, c, bit;
    sudoku_t newS;
    unsigned char *answer;

    do {
        updated = false;
        solved = true;

        for(i = 0; i < 81; i++){
            if(s->answer[i] != 0){
                continue;
            }
            solved = false;

            candidate = _get_candidate_bit(s, i);
            if (candidate == 0){
                return NULL;
            }

            lowest = _lowest_one_bit(candidate);
            if (candidate == lowest) {
                _update(s, i, _to_num(lowest));
                updated = true;
            }
        }
    }while(updated);

    if(solved){
        answer = malloc(sizeof(unsigned char) * 81);
        memcpy(answer, s->answer, sizeof(unsigned char) * 81);
        return answer;
    }

    count = INT_MAX;
    index = 0;
    for(i = 0; i < 81; i++){
        if(s->answer[i] != 0){
            continue;
        }

        c = _bit_count(_get_candidate_bit(s, i));
        if (c < count){
            count = c;
            index = i;
        }
    }
    
	// 複数ある候補のうちの一つを仮定して、再帰的に解析
    candidate = _get_candidate_bit(s, index);
    for(i = 0; i < 9; i++){
        bit = candidate & (1 << i);
        if(bit == 0){
            continue;
        }

        memcpy(&newS, s, sizeof(sudoku_t));
        _update(&newS, index, i + 1);

        if((answer = _solve_internal(&newS)) != NULL){
            return answer;
        }
    }

    return NULL;
}

void _update(sudoku_t *s, int index, unsigned char number) {
    int r, c;
    unsigned short value;

    s->answer[index] = number;

    r = index / 9;
    c = index % 9;
    value = (1 << (number - 1));
    s->rows[r] |= value;
    s->columns[c] |= value;
    s->regions[(r / 3) * 3 + (c / 3)] |= value;
}

int _get_candidate_bit(sudoku_t *s, int index) {
    int r, c;
    unsigned short bit;

    r = index / 9;
    c = index % 9;

    bit = 0;
    bit |= s->rows[r];
    bit |= s->columns[c];
    bit |= s->regions[(r / 3) * 3 + (c / 3)];

    return ~bit & 0x1FF;
}

int _bit_count(unsigned short value) {
    int sum = 0;
    int i;

    for(i = 0; i < 9; i++){
        if((value & 1 << i) != 0){
            sum++;
        }
    }

    return sum;
}

unsigned short _lowest_one_bit(unsigned short i) {
    return i & -i;
}

unsigned char _to_num(int bit){
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
		fprintf(stderr, "Invalid bit.");
        exit(1);
	}
}
