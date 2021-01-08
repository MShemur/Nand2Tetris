// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Mult.asm

// Multiplies R0 and R1 and stores the result in R2.
// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)

// Put your code here.
@R2
M=0
@R0
D=M
@number
M=D
@R1
D=M
@n
M=D
@i
M=0
@mult
M=0

(LOOP)
@i  //1//2//3
D=M
@n//2
D=D-M //d=i-n  //-1//0//1
@STOP
D;JGE

@number//2
D=M
@mult//2//4
M=M+D
@i//
M=M+1//2//3
@LOOP
0;JMP


// @COUNT
// 0;JMP

(STOP)
@mult
D=M
@R2
M=D
@END
0;JMP

(END)
@END
0;JMP



// @R0
// D=M
// @n
// M=D
// @i
// M=1
// @sum
// M=0

// (LOOP)
// @i
// D=M
// @n
// D=D-M
// @STOP
// D;JGT

// @sum
// D=M
// @i
// D=D+M
// @sum
// M=D
// @i
// M=M+1
// @LOOP
// 0;JMP

// (STOP)
// @sum
// D=M
// @R1
// M=D










// (END)
// @END
// 0;JMP


// @R0
// D=M

// @POSITIVE
// D;JGT

// @R1
// M=0
// @END
// 0;JMP

// (POSITIVE)
// @R1
// M=1

// (ENDD)
// @ENDD
// 0;JMP