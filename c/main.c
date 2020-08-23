
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include <time.h>
#include <malloc.h>

#define BUFFER_LENGTH 1024 * 8

extern unsigned char* solve(unsigned char *quiz);

void _read(char *file, int rows, unsigned char **quizzes, unsigned char **solutions);
void _solve(int rows, unsigned char **quizzes, unsigned char **solutions);
bool _valid(unsigned char *answer, unsigned char *solution);
void _export(unsigned char *answer);

int main(int argc, char *argv[])
{
    char* file = argv[1];
    int rows = atoi(argv[2]);

    unsigned char **quizzes;
    unsigned char **solutions;

    quizzes = malloc(sizeof(unsigned char*) * rows);
    solutions = malloc(sizeof(unsigned char*) * rows);

    fprintf(stderr, "Start read...\n");
    struct timespec startRead, endRead;
    clock_gettime(CLOCK_MONOTONIC, &startRead);
    _read(file, rows, quizzes, solutions);
    clock_gettime(CLOCK_MONOTONIC, &endRead);
    
    fprintf(stderr, "Start solve...\n");
    struct timespec startSolve,  endSolve;
    clock_gettime(CLOCK_MONOTONIC, &startSolve);
    _solve(rows, quizzes, solutions);
    clock_gettime(CLOCK_MONOTONIC, &endSolve);

    double readTime = (endRead.tv_sec - startRead.tv_sec) * 1000 + (endRead.tv_nsec - startRead.tv_nsec) / 1000000.0;
    double solveTime = (endSolve.tv_sec - startSolve.tv_sec) * 1000 + (endSolve.tv_nsec - startSolve.tv_nsec) / 1000000.0;
    fprintf(stderr, "%lfms, %lfms\n", readTime, solveTime);
}

void _read(char* file, int rows, unsigned char **quizzes, unsigned char **solutions) {
    FILE* fp;
    char line[1024];
    char *io_buffer;

    unsigned char *quiz, *solution;
    
    int i, j;
    int index;

    if((fp = fopen(file, "r")) == NULL) {
        fprintf(stderr, "File can't open.\n");
        exit(1);
    }
    
    io_buffer = malloc(sizeof(char) * BUFFER_LENGTH);
    setvbuf(fp, io_buffer, _IOFBF, sizeof(char) * BUFFER_LENGTH); 

    // 1行目をスキップ
    if(!fgets(line, sizeof(line), fp)){
        fprintf(stderr, "File can't read.\n");
        exit(1);
    }

    for(i = 0; i < rows; i++) {
        if(!fgets(line, sizeof(line), fp)){  
            fprintf(stderr, "File can't read.\n");
            exit(1);
        }
        index = 0;

        // 問題
        quiz = malloc(sizeof(unsigned char*) * 81);
        for(j = 0; j < 81; j++) {
            quiz[j] = (unsigned char) (line[index++] - '0');
        }
        quizzes[i] = quiz;

        // カンマ
        index++;

        // 回答
        solution = malloc(sizeof(unsigned char*) * 81);
        for(j = 0; j < 81; j++){
            solution[j] = (unsigned char) (line[index++] - '0');
        }
        solutions[i] = solution;
    }

    fclose(fp);
    free(io_buffer);
}

void _solve(int rows, unsigned char **quizzes, unsigned char **solutions){
    int i;
    unsigned char *answer;

    for(i = 0; i < rows; i++){
        answer = solve(quizzes[i]);
        
        if (answer == NULL || !_valid(answer, solutions[i])) {
            fprintf(stderr, "Invalid answer. [index=%d]\n", i);
            exit(1);
        }

        // _export(answer);

        free(answer);
    }
}

bool _valid(unsigned char *answer, unsigned char *solution) {
    int i;
    for(i = 0; i < 81; i++) {
        if(answer[i] != solution[i]) {
            return false;
        }
    }

    return true;
}

void _export(unsigned char *answer){
    int i;
    char builder[82];

    for(i = 0; i < 81; i++){
        builder[i] = answer[i] + '0';
    }
    builder[81] = '\0';

    printf("%s\n", builder);
}