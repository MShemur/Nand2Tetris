// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Fill.asm

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel;
// the screen should remain fully black as long as the key is pressed. 
// When no key is pressed, the program clears the screen, i.e. writes
// "white" in every pixel;
// the screen should remain fully clear as long as no key is pressed.

// Put your code here.
@KBD
D=A
@keyaddress
M=D

@8191
D=A
@wordscount//8191
M=D

(LISTENTOKEY)
@i
M=0
@SCREEN
D=A
@addr//16384
M=D
@keyaddress
A=M
D=M
@DRAW
D;JNE
@CLEAR
0;JMP

(DRAW)
@addr
A=M
M=-1
@1
D=A
@addr
M=D+M
@i
M=M+1
@LOOPFORDRAW
0;JMP

(CLEAR)
@addr
A=M
M=0
@1
D=A
@addr
M=D+M
@i
M=M+1
@LOOPFORCLEAR
0;JMP

(LOOPFORDRAW)
@i  //1//2//3
D=M
@wordscount//2
D=M-D //d=i-n  //-1//0//1
@LISTENTOKEY
D;JLT
@DRAW
0;JMP

(LOOPFORCLEAR)
@i  //1//2//3
D=M
@wordscount//2
D=M-D //d=i-n  //-1//0//1
@LISTENTOKEY
D;JLT
@CLEAR
0;JMP

(END)
@END
0;JMP
