using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualMachine
{
    public class CommandParser
    {
        public bool HasMoreCommands => currentCommand < lines.Count;

        public CommandType CommandTyp { get; set; }
        public string Argument1 { get; set; }
        public int Argument2 { get; set; }


        private int currentCommand = 0;

        private readonly List<string> lines;

        private readonly string[] arithmeticCommands = { "neg", "sub", "add", "eq", "gt", "lt", "and", "or", "not" };

        public CommandParser(List<string> lines)
        {
            this.lines = lines;
        }

        public void Advance()
        {
            var args = lines[currentCommand].Split(' ');

            if (arithmeticCommands.Contains(args[0]))
            {
                CommandTyp = CommandType.Arithmetic;
                Argument1 = args[0];
            }
            else if (args[0].Equals("pop"))
            {
                CommandTyp = CommandType.Pop;
                Argument1 = args[1];
                Argument2 = int.Parse(args[2]);
            }
            else if (args[0].Equals("push"))
            {
                CommandTyp = CommandType.Push;
                Argument1 = args[1];
                Argument2 = int.Parse(args[2]);
            }
            else if (args[0].Equals("if-goto"))
            {
                CommandTyp = CommandType.If;
                Argument1 = args[1];
            }
            else if (args[0].Equals("goto"))
            {
                CommandTyp = CommandType.GoTo;
                Argument1 = args[1];
            }
            else if (args[0].Equals("label"))
            {
                CommandTyp = CommandType.Label;
                Argument1 = args[1];
            }
            else if (args[0].Equals("function"))
            {
                CommandTyp = CommandType.Function;
                Argument1 = args[1];
                Argument2 = int.Parse(args[2]);
            }
            else if (args[0].Equals("call"))
            {
                CommandTyp = CommandType.Call;
                Argument1 = args[1];
                Argument2 = int.Parse(args[2]);
            }
            else if (args[0].Equals("return"))
            {
                CommandTyp = CommandType.Return;
            }

            currentCommand++;
        }
    }
}
