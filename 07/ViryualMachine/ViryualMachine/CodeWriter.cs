using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualMachine
{
    public class CodeWriter
    {
        public List<string> OutputLines { get; set; }

        private int cycleNumber = 0;

        private Dictionary<string, int> callCount;
        private string currentFunction = "";

        public CodeWriter()
        {
            OutputLines = new List<string>();
            callCount = new Dictionary<string, int>();
        }

        public void WriteArithmetic(string command)
        {
            StringBuilder pushPointer = new StringBuilder();

            OutputLines.Add("\n//" + command);

            if (command == "add")
            {
                pushPointer.Append("@SP\n");
                pushPointer.Append("AM=M-1\n");
                pushPointer.Append("D=M\n");
                pushPointer.Append("@SP\n");
                pushPointer.Append("A=M-1\n");
                pushPointer.Append("M=M+D");
            }

            else if (command == "sub")
            {
                pushPointer.Append("@SP\n");
                pushPointer.Append("AM=M-1\n");
                pushPointer.Append("D=M\n");
                pushPointer.Append("@SP\n");
                pushPointer.Append("A=M-1\n");
                pushPointer.Append("M=M-D");
            }

            else if (command == "eq")
            {
                pushPointer.Append("@SP\n");
                pushPointer.Append("AM=M-1\n");
                pushPointer.Append("D=M\n");
                pushPointer.Append("@SP\n");
                pushPointer.Append("A=M-1\n");
                pushPointer.Append("M=M-D\n");
                pushPointer.Append("D=M\n");

                pushPointer.Append($"@TRUE{cycleNumber}\n");
                pushPointer.Append("D;JEQ\n");
                pushPointer.Append("@R14\n");
                pushPointer.Append("M=0\n");
                pushPointer.Append("D=M\n");

                pushPointer.Append($"@FALSE{cycleNumber}\n");
                pushPointer.Append("0;JMP\n");

                pushPointer.Append($"(TRUE{cycleNumber})\n");
                pushPointer.Append("@R13\n");
                pushPointer.Append("M=-1\n");
                pushPointer.Append("D=M\n");

                pushPointer.Append($"(FALSE{cycleNumber})\n");
                pushPointer.Append("@SP\n");
                pushPointer.Append("A=M-1\n");
                pushPointer.Append("M=D\n");
            }

            else if (command == "lt")
            {
                pushPointer.Append("@SP\n");
                pushPointer.Append("AM=M-1\n");
                pushPointer.Append("D=M\n");
                pushPointer.Append("@SP\n");
                pushPointer.Append("A=M-1\n");
                pushPointer.Append("M=M-D\n");
                pushPointer.Append("D=M\n");

                pushPointer.Append($"@TRUE{cycleNumber}\n");
                pushPointer.Append("D;JLT\n");
                pushPointer.Append("@R14\n");
                pushPointer.Append("M=0\n");
                pushPointer.Append("D=M\n");

                pushPointer.Append($"@FALSE{cycleNumber}\n");
                pushPointer.Append("0;JMP\n");

                pushPointer.Append($"(TRUE{cycleNumber})\n");
                pushPointer.Append("@R13\n");
                pushPointer.Append("M=-1\n");
                pushPointer.Append("D=M\n");

                pushPointer.Append($"(FALSE{cycleNumber})\n");
                pushPointer.Append("@SP\n");
                pushPointer.Append("A=M-1\n");
                pushPointer.Append("M=D\n");
            }

            else if (command == "gt")
            {
                pushPointer.Append("@SP\n");
                pushPointer.Append("AM=M-1\n");
                pushPointer.Append("D=M\n");
                pushPointer.Append("@SP\n");
                pushPointer.Append("A=M-1\n");
                pushPointer.Append("M=M-D\n");
                pushPointer.Append("D=M\n");

                pushPointer.Append($"@TRUE{cycleNumber}\n");
                pushPointer.Append("D;JGT\n");
                pushPointer.Append("@R14\n");
                pushPointer.Append("M=0\n");
                pushPointer.Append("D=M\n");

                pushPointer.Append($"@FALSE{cycleNumber}\n");
                pushPointer.Append("0;JMP\n");

                pushPointer.Append($"(TRUE{cycleNumber})\n");
                pushPointer.Append("@R13\n");
                pushPointer.Append("M=-1\n");
                pushPointer.Append("D=M\n");

                pushPointer.Append($"(FALSE{cycleNumber})\n");
                pushPointer.Append("@SP\n");
                pushPointer.Append("A=M-1\n");
                pushPointer.Append("M=D\n");
            }

            else if (command == "and")
            {
                pushPointer.Append("@SP\n");
                pushPointer.Append("AM=M-1\n");
                pushPointer.Append("D=M\n");
                pushPointer.Append("@SP\n");
                pushPointer.Append("A=M-1\n");
                pushPointer.Append("M=M&D\n");
            }

            else if (command == "or")
            {
                pushPointer.Append("@SP\n");
                pushPointer.Append("AM=M-1\n");
                pushPointer.Append("D=M\n");
                pushPointer.Append("@SP\n");
                pushPointer.Append("A=M-1\n");
                pushPointer.Append("M=M|D\n");
            }
            else if (command == "not")
            {
                pushPointer.Append("@SP\n");
                pushPointer.Append("A=M-1\n");
                pushPointer.Append("M=!M\n");
            }

            else if (command == "neg")
            {
                pushPointer.Append("@SP\n");
                pushPointer.Append("A=M-1\n");
                pushPointer.Append("M=-M\n");
            }
            OutputLines.Add(pushPointer.ToString());

            cycleNumber++;
            /*
            neg

            @SP
            A=M-1
            M=-M

            not

            @SP
               AM=M-1
               D=M
               @1
               D=D+A
               
               @TRUE
               D;JEQ
               @SP
               A=M
               M=-1
               @FALSE
               0;JMP
               
               (TRUE)
               @SP
               A=M
               M=0
               
               (FALSE)



            or
            @SP
               AM=M-1
               D=M
               @SP
               A=M-1
               M=M+D
               D=M
               @1
               D=D+A
               
               @RR13
               D;JLE
               @R14
               M=0
               D=M
               @RR14
               0;JMP
               
               (RR13)
               @R13
               M=-1
               D=M
               
               (RR14)
               @SP
               A=M-1
               M=D


            and

            @SP
@SP
               AM=M-1
               D=M
               @SP
               A=M-1
               M=M&D
            
            
            
            eq

                @SP
               AM=M-1
               D=M
               @SP
               A=M-1
               M=M-D
               D=M
               
               @TRUE
               D;JEQ
               @R14
               M=0
               D=M

               @FALSE
               0;JMP
               
               (TRUE)
               @R13
               M=-1
               D=M
               
               (FALSE)
               @SP
               A=M-1
               M=D

             ADD

            @SP
               AM=M-1
               D=M
               @SP
               A=M-1
               M=M+D

            SUB

            @SP
               AM=M-1
               D=M
               @SP
               A=M-1
               M=M-D


               
            gt

               @SP
               AM=M-1
               D=M
               @SP
               A=M-1
               M=M-D
               D=M
               
               @TRUE
               D;JGT
               @R14
               M=0
               D=M
               
               @FALSE
               0;JMP
               
               (TRUE)
               @R13
               M=-1
               D=M
               
               (FALSE)
               @SP
               A=M-1
               M=D

            lt

               @SP
               AM=M-1
               D=M
               @SP
               A=M-1
               M=M-D
               D=M
               
               @TRUE
               D;JLT
               @R14
               M=0
               D=M
               
               @FALSE
               0;JMP
               
               (TRUE)
               @R13
               M=-1
               D=M
               
               (FALSE)
               @SP
               A=M-1
               M=D
             */
        }

        public void WritePushPop(CommandType commandType, string segment, int index)
        {

            OutputLines.Add("\n//" + commandType + " " + segment + " " + index);

            StringBuilder line = new StringBuilder();
            string[] standardSegments = { "local", "argument", "this", "that" };

            if (standardSegments.Contains(segment))
                switch (commandType)
                {
                    case CommandType.Pop:
                        line.Append(GetStandardPop(segment, index));
                        break;
                    case CommandType.Push:
                        line.Append(GetStandardPush(segment, index));
                        break;
                }

            else if (segment == "constant")
                line.Append(GetPushConstant(segment, index));

            else if (segment == "static")
                switch (commandType)
                {
                    case CommandType.Pop:
                        line.Append(GetPopStatic(segment, index));
                        break;
                    case CommandType.Push:
                        line.Append(GetPushStatic(segment, index));
                        break;
                }

            else if (segment == "temp")
                switch (commandType)
                {
                    case CommandType.Pop:
                        line.Append(GetPopTemp(segment, index));
                        break;
                    case CommandType.Push:
                        line.Append(GetPushTemp(segment, index));
                        break;
                }
            else if (segment == "pointer")
                switch (commandType)
                {
                    case CommandType.Pop:
                        line.Append(GetPopPointer(segment, index));
                        break;
                    case CommandType.Push:
                        line.Append(GetPushPointer(segment, index));
                        break;
                }

            OutputLines.Add(line.ToString());

            /*
            pop local 2

                @2
               D=A
               @LCL
               A=D+M
               D=A
               @R13
               M=D
               @SP
               AM=M-1
               D=M
               @R13
               A=M
               M=D

            push local 2

                @2
               D=A
               @LCL
               A=D+M
               D=M
               @SP
               A=M
               M=D
               @SP
               M=M+1

            push const 1

                 @1
               D=A
               @SP
               A=M
               M=D
               @SP
               M=M+1

            pop temp 2

            @2
               D=A
               @5
               D=D+A
               @R13
               M=D
               @SP
               AM=M-1
               D=M
               @R13
               A=M
               M=D

            push temp 2

            @2
               D=A
               @5
               A=D+A
               D=M
               @SP
               A=M
               M=D
               @SP
               M=M+1

            push pointer 0

            @THIS
               D=M
               @SP
               A=M
               M=D
               @SP
               M=M+1

          push pointer 1
               
               @THAT
               D=M
               @SP
               A=M
               M=D
               @SP
               M=M+1

            // pop pointer 1
               @SP
               M=M-1
               @SP
               A=M
               D=M
               @THAT
               M=D

            // pop pointer 0
               @SP
               M=M-1
               @SP
               A=M
               D=M
               @THIS
               M=D

            pop static 2

                @Foo.2
               D=A
               @R13
               M=D
               @SP
               AM=M-1
               D=M
               @R13
               A=M
               M=D

            push static 2

            @Foo.2
               D=M
               @SP
               A=M
               M=D
               @SP
               M=M+1
            */
        }

        #region Push and Pops
        private StringBuilder GetPushPointer(string segment, in int index)
        {
            StringBuilder pushPointer = new StringBuilder();

            if (index == 0)
            {
                pushPointer.Append("@THIS\n");
            }
            else if (index == 1)
            {
                pushPointer.Append("@THAT\n");
            }
            pushPointer.Append("D=M\n");
            pushPointer.Append("@SP\n");
            pushPointer.Append("A=M\n");
            pushPointer.Append("M=D\n");
            pushPointer.Append("@SP\n");
            pushPointer.Append("M=M+1");

            return pushPointer;

            /*
             push pointer 1
               
               @THAT
               D=M
               @SP
               A=M
               M=D
               @SP
               M=M+1
             */
        }

        private StringBuilder GetPopPointer(string segment, in int index)
        {
            StringBuilder popPointer = new StringBuilder();

            popPointer.Append("@SP\n");
            popPointer.Append("M=M-1\n");
            popPointer.Append("@SP\n");
            popPointer.Append("A=M\n");
            popPointer.Append("D=M\n");
            if (index == 0)
            {
                popPointer.Append("@THIS\n");
            }
            else if (index == 1)
            {
                popPointer.Append("@THAT\n");
            }
            popPointer.Append("M=D");

            return popPointer;

            /*
              pop pointer 0
               @SP
               M=M-1
               @SP
               A=M
               D=M
               @THIS
               M=D
             */
        }

        private StringBuilder GetPushTemp(string segment, in int index)
        {
            StringBuilder pushTemp = new StringBuilder();

            pushTemp.Append($"@{index}\n");
            pushTemp.Append("D=A\n");
            pushTemp.Append("@5\n");
            pushTemp.Append("A=D+A\n");
            pushTemp.Append("D=M\n");
            pushTemp.Append("@SP\n");
            pushTemp.Append("A=M\n");
            pushTemp.Append("M=D\n");
            pushTemp.Append("@SP\n");
            pushTemp.Append("M=M+1");

            return pushTemp;

            /*
            push temp 2
               
               @2
               D=A
               @5
               A=D+A
               D=M
               @SP
               A=M
               M=D
               @SP
               M=M+1
             */
        }

        private StringBuilder GetPopTemp(string segment, in int index)
        {
            StringBuilder popTemp = new StringBuilder();

            popTemp.Append($"@{index}\n");
            popTemp.Append("D=A\n");
            popTemp.Append("@5\n");
            popTemp.Append("D=D+A\n");
            popTemp.Append("@R13\n");
            popTemp.Append("M=D\n");
            popTemp.Append("@SP\n");
            popTemp.Append("AM=M-1\n");
            popTemp.Append("D=M\n");
            popTemp.Append("@R13\n");
            popTemp.Append("A=M\n");
            popTemp.Append("M=D");


            return popTemp;

            /*
               pop temp 2
               
               @2
               D=A
               @5
               D=D+A
               @R13
               M=D
               @SP
               AM=M-1
               D=M
               @R13
               A=M
               M=D
             */
        }

        private StringBuilder GetPushStatic(string segment, in int index)
        {
            StringBuilder pushStatic = new StringBuilder();
            var file = currentFunction.Substring(0, currentFunction.IndexOf('.'));

            pushStatic.Append($"@{file}.{index}\n");
            pushStatic.Append("D=M\n");
            pushStatic.Append("@SP\n");
            pushStatic.Append("A=M\n");
            pushStatic.Append("M=D\n");
            pushStatic.Append("@SP\n");
            pushStatic.Append("M=M+1");

            return pushStatic;

            /*
             push static 2
            
               @Foo.2
               D=M
               @SP
               A=M
               M=D
               @SP
               M=M+1
               */
        }

        private StringBuilder GetPopStatic(string segment, in int index)
        {
            StringBuilder popStatic = new StringBuilder();
            var file = currentFunction.Substring(0, currentFunction.IndexOf('.') );
            popStatic.Append($"@{file}.{index}\n");
            popStatic.Append("D=A\n");
            popStatic.Append("@R13\n");
            popStatic.Append("M=D\n");
            popStatic.Append("@SP\n");
            popStatic.Append("AM=M-1\n");
            popStatic.Append("D=M\n");
            popStatic.Append("@R13\n");
            popStatic.Append("A=M\n");
            popStatic.Append("M=D");

            return popStatic;

            /*
             pop static 2
               
               @Foo.2
               D=A
               @R13
               M=D
               @SP
               AM=M-1
               D=M
               @R13
               A=M
               M=D
             */
        }

        private string GetPushConstant(string segment, in int index)
        {
            StringBuilder pushConstant = new StringBuilder();

            //  pushConstant.Append($"\n//push constant {index}\n");


            pushConstant.Append($"@{index}\n");
            pushConstant.Append("D=A\n");
            pushConstant.Append("@SP\n");
            pushConstant.Append("A=M\n");
            pushConstant.Append("M=D\n");
            pushConstant.Append("@SP\n");
            pushConstant.Append("M=M+1");

            return pushConstant.ToString();
            /*
               @1
               D=A
               @SP
               A=M
               M=D
               @SP
               M=M+1
             */
        }

        private string GetStandardPush(string segment, in int index)
        {
            StringBuilder pushCommand = new StringBuilder();

            pushCommand.Append($"@{index}\n");
            pushCommand.Append("D=A\n");

            switch (segment)
            {
                case "local":
                    pushCommand.Append("@LCL\n");
                    break;
                case "argument":
                    pushCommand.Append("@ARG\n");
                    break;
                case "this":
                    pushCommand.Append("@THIS\n");
                    break;
                case "that":
                    pushCommand.Append("@THAT\n");
                    break;
            }

            pushCommand.Append("A=D+M\n");
            pushCommand.Append("D=M\n");
            pushCommand.Append("@SP\n");
            pushCommand.Append("A=M\n");
            pushCommand.Append("M=D\n");
            pushCommand.Append("@SP\n");
            pushCommand.Append("M=M+1");

            return pushCommand.ToString();

            /*
               push local 2
               
               @2
               D=A
               @LCL
               A=D+M
               D=M
               @SP
               A=M
               M=D
               @SP
               M=M+1
             */
        }

        private string GetStandardPop(string segment, in int index)
        {
            StringBuilder popCommand = new StringBuilder();

            popCommand.Append($"@{index}\n");
            popCommand.Append("D=A\n");

            switch (segment)
            {
                case "local":
                    popCommand.Append("@LCL\n");
                    break;
                case "argument":
                    popCommand.Append("@ARG\n");
                    break;
                case "this":
                    popCommand.Append("@THIS\n");
                    break;
                case "that":
                    popCommand.Append("@THAT\n");
                    break;
            }

            popCommand.Append("A=D+M\n");
            popCommand.Append("D=A\n");
            popCommand.Append("@R13\n");
            popCommand.Append("M=D\n");
            popCommand.Append("@SP\n");
            popCommand.Append("AM=M-1\n");
            popCommand.Append("D=M\n");
            popCommand.Append("@R13\n");
            popCommand.Append("A=M\n");
            popCommand.Append("M=D");

            return popCommand.ToString();

            /*
              pop local 2
               
               @2
               D=A
               @LCL
               A=D+M
               D=A
               @R13
               M=D
               @SP
               AM=M-1
               D=M
               @R13
               A=M
               M=D
             */
        }
        #endregion

        public void WriteIf(string label)
        {
            label = currentFunction + "$" + label;
            StringBuilder writeIf = new StringBuilder();

            OutputLines.Add("\n//if-goto " + label);

            writeIf.Append("@SP\n");
            writeIf.Append("AM=M-1\n");
            writeIf.Append("D=M\n");
            writeIf.Append($"@{label}\n");
            writeIf.Append("D;JNE");

            OutputLines.Add(writeIf.ToString());
        }

        public void WriteLabel(string label)
        {
            label = currentFunction + "$" + label;

            StringBuilder writeLabel = new StringBuilder();

            OutputLines.Add("\n//label " + label);

            writeLabel.Append($"({label})");

            OutputLines.Add(writeLabel.ToString());
        }

        public void WriteGoTo(string label)
        {
            label = currentFunction + "$" + label;

            StringBuilder writeGoTo = new StringBuilder();

            OutputLines.Add("\n//goto " + label);

            writeGoTo.Append($"@{label}\n");
            writeGoTo.Append("0;JMP");

            OutputLines.Add(writeGoTo.ToString());
        }

        public void WriteFunction(string name, int numVars)
        {
            currentFunction = name;
            StringBuilder writeFunction = new StringBuilder();

            OutputLines.Add("\n//function " + name + " " + numVars);

            writeFunction.Append($"({name})\n");
            while (numVars > 0)
            {

                writeFunction.Append("@0\n");
                writeFunction.Append("D=A\n");

                writeFunction.Append("@SP\n");
                writeFunction.Append("A=M\n");
                writeFunction.Append("M=D\n");
                writeFunction.Append("@SP\n");
                writeFunction.Append("M=M+1");

                writeFunction.Append("\n");
                numVars--;
            }

            OutputLines.Add(writeFunction.ToString());
        }

        public void WriteReturn()
        {
            StringBuilder writeFunction = new StringBuilder();

            OutputLines.Add("\n//return");

            new[]{
                "@LCL",
                "\n",
                "D=M",
                "\n",
                "@EndFrame",
                "\n",
                "M=D",
                "\n",
                "@EndFrame",
                "\n",
                "D=M",
                "\n",
                "@5",
                "\n",
                "A=D-A",
                "\n",
                "D=M",
                "\n",
                "@retAddress",
                "\n",
                "M=D",
                "\n",

                "@SP",
                "\n",
                "A=M-1",
                "\n",
                "D=M",
                "\n",
                "@ARG",
                "\n",
                "A=M",
                "\n",
                "M=D",
                "\n",
                "//SP=ARG+1",
                "\n",
                "@ARG",
                "\n",
                "D=M",
                "\n",
                "@SP",
                "\n",
                "M=D+1",
                "\n",

                "@EndFrame",
                "\n",
                "D=M",
                "\n",
                "@1",
                "\n",
                "A=D-A",
                "\n",
                "D=M",
                "\n",
                "@THAT",
                "\n",
                "M=D",
                "\n",

                "@EndFrame",
                "\n",
                "D=M",
                "\n",
                "@2",
                "\n",
                "A=D-A",
                "\n",
                "D=M",
                "\n",
                "@THIS",
                "\n",
                "M=D",
                "\n",

                "@EndFrame",
                "\n",
                "D=M",
                "\n",
                "@3",
                "\n",
                "A=D-A",
                "\n",
                "D=M",
                "\n",
                "@ARG",
                "\n",
                "M=D",
                "\n",

                "@EndFrame",
                "\n",
                "D=M",
                "\n",
                "@4",
                "\n",
                "A=D-A",
                "\n",
                "D=M",
                "\n",
                "@LCL",
                "\n",
                "M=D",
                "\n",

                "@retAddress",
                "\n",
                "A=M",
                "\n",
                "0;JMP"}.ToList().ForEach(x => writeFunction.Append(x));


            OutputLines.Add(writeFunction.ToString());
            //currentFunction = "";
        }

        public void WriteCall(string name, int numArgs)
        {
            int commandCount = 1;
            if (callCount.TryGetValue(name, out int count))
            {
                commandCount = count + 1;
                callCount[name] = commandCount;
            }
            else
                callCount.Add(name, 1);



            StringBuilder writeCall = new StringBuilder();

            OutputLines.Add("\n//Call " + name + " " + numArgs + "\n");

            new[]{
                $"@{name}$ret.{commandCount}",
                "\n",
                "D=A",
                "\n",
                "@SP",
                "\n",
                "A=M",
                "\n",
                "M=D",
                "\n",
                "@SP",
                "\n",
                "M=M+1",
                "\n",
                "@LCL",
                "\n",
                "D=M",
                "\n",
                "@SP",
                "\n",
                "A=M",
                "\n",
                "M=D",
                "\n",

                "@SP",
                "\n",
                "M=M+1",
                "\n",
                "@ARG",
                "\n",
                "D=M",
                "\n",
                "@SP",
                "\n",
                "A=M",
                "\n",
                "M=D",
                "\n",

                "@SP",
                "\n",
                "M=M+1",
                "\n",
                "@THIS",
                "\n",
                "D=M",
                "\n",
                "@SP",
                "\n",
                "A=M",
                "\n",
                "M=D",
                "\n",

                "@SP",
                "\n",
                "M=M+1",
                "\n",
                "@THAT",
                "\n",
                "D=M",
                "\n",
                "@SP",
                "\n",
                "A=M",
                "\n",
                "M=D",
                "\n",
                "@SP",
                "\n",
                "M=M+1",
                                "\n",
                $"@{numArgs}",
                "\n",
                "D=A",
                "\n",
                "@5",
                "\n",
                "D=D+A",
                "\n",
                "@SP",
                "\n",
                "D=M-D",
                "\n",
                "@ARG",
                "\n",
                "M=D",
                "\n",
                "@SP",
                "\n",
                "D=M",
                "\n",
                "@LCL",
                "\n",
                "M=D",
                "\n",
                $"@{name}",
                //"\n",
                //"A=M",
                "\n",
                "0;JMP",
                "\n",
                $"({name}$ret.{commandCount})",

            }.ToList().ForEach(x => writeCall.Append(x));


            OutputLines.Add(writeCall.ToString());
        }

        public void WriteBootstrap()
        {
            StringBuilder writeBootstrap = new StringBuilder();

            OutputLines.Add("\n//Bootstrap\n");

            new[]{
                "@256",
                "\n",
                "D=A",
                "\n",
                "@SP",
                "\n",
                "M=D",
                "\n",

            }.ToList().ForEach(x => writeBootstrap.Append(x));

            OutputLines.Add(writeBootstrap.ToString());

            WriteCall("Sys.init", 0);
        }
    }
}
