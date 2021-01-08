using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    class CodeGetter
    {
        private List<string> inputCommands;

        private List<string> commandsBin;
        private Dictionary<string, int> symbolTable;
        public CodeGetter(List<string> commands)
        {
            inputCommands = commands;
            commandsBin = new List<string>();
            symbolTable = CreateSymbolTable();
        }

        private Dictionary<string, int> CreateSymbolTable()
        {
            symbolTable = new Dictionary<string, int>()
            {
                {"R0", 0},
                {"R1", 1},
                {"R2", 2},
                {"R3", 3},
                {"R4", 4},
                {"R5", 5},
                {"R6", 6},
                {"R7", 7},
                {"R8", 8},
                {"R9", 9},
                {"R10", 10},
                {"R11", 11},
                {"R12", 12},
                {"R13", 13},
                {"R14", 14},
                {"R15", 15},
                {"SCREEN", 16384},
                {"KBD", 24576},
                {"SP", 0},
                {"LCL", 1},
                {"ARG", 2},
                {"THIS", 3},
                {"THAT", 4},
            };

            return symbolTable;
        }

        public List<string> GetBinaryCommands()
        {
            commandsBin = Parse(inputCommands);
            return commandsBin;
        }

        private List<string> Parse(List<string> commands)
        {
            List<string> binary = new List<string>();
            commands = GetWithoutLabels(commands, symbolTable);
            AddSymbols(commands, symbolTable);
            commands = GetPure(commands, symbolTable);
            foreach (var line in commands)
            {
                binary.Add(ParseFromPure(line));
            }

            return binary;
        }

        /// <summary>
        /// Возвращаем строки без ссылок и лайбелов
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private List<string> GetPure(List<string> commands, Dictionary<string, int> dictionary)
        {
            List<string> pureString = new List<string>();
            foreach (var command in commands)
            {
                if (command.StartsWith("@"))
                {
                    //   int.TryParse(command.Substring(1), out int number);
                    var aInstruction = command.Substring(1, command.Length - 1);

                    if (aInstruction.All(char.IsDigit))
                    {
                        pureString.Add(command);
                        continue;
                    }

                    symbolTable.TryGetValue(aInstruction, out int number);
                    pureString.Add("@" + number);
                    continue;
                }
                pureString.Add(command);
            }

            return pureString;
        }

        private void AddSymbols(List<string> commands, Dictionary<string, int> dictionary)
        {
            int commandCounter = 16;

            foreach (var command in commands)
            {
                if (command.StartsWith("@"))
                {
                    //   int.TryParse(command.Substring(1), out int number);
                    var aInstruction = command.Substring(1, command.Length - 1);

                    if (aInstruction.All(char.IsDigit)) continue;

                    if (symbolTable.ContainsKey(aInstruction)) continue;

                    symbolTable.Add(aInstruction, commandCounter);
                    commandCounter++;
                }
            }
        }

        private List<string> GetWithoutLabels(List<string> commands, Dictionary<string, int> dictionary)
        {
            List<string> commandsWithoutLabels = new List<string>();
            int commandCounter = 0;
            foreach (var command in commands)
            {
                if (command.StartsWith("(") && command.EndsWith(")"))
                {
                    symbolTable.Add(command.Substring(1, command.Length - 2), commandCounter);
                    continue;
                }
                commandsWithoutLabels.Add(command);
                commandCounter++;
            }

            return commandsWithoutLabels;
        }

        private string ParseFromPure(string line)
        {
            string command = "";
            if (line.Substring(0, 1) == "@")
            {
                command = GetBinary(line.Substring(1));
                string zeros = new string('0', 16 - command.Length);
                command = zeros + command;
            }
            else
            {
                string[] substrings;
                string comp = "";
                string jump = "";
                substrings = line.Split('=', ';');
                string dest = "";

                if (line.Contains('='))
                {
                    dest = GetDest(substrings[0]);
                    jump = GetJump("");
                    comp = GetComp(substrings[1]);
                }
                if (line.Contains(';'))
                {
                    dest = GetDest("");
                    comp = GetComp(substrings[0]);
                    jump = GetJump(substrings[1]);
                }

                command = "111" + comp + dest + jump;
            }

            return command;
        }

        private string GetJump(string jump)
        {
            if (jump.ToUpper().Equals("JGT"))
                return "001";
            else if (jump.ToUpper().Equals("JEQ"))
                return "010";
            else if (jump.ToUpper().Equals("JGE"))
                return "011";
            else if (jump.ToUpper().Equals("JLT"))
                return "100";
            else if (jump.ToUpper().Equals("JNE"))
                return "101";
            else if (jump.ToUpper().Equals("JLE"))
                return "110";
            else if (jump.ToUpper().Equals("JMP"))
                return "111";
            else if (jump.ToUpper().Equals(""))
                return "000";
            else if (jump.ToUpper().Equals("0"))
                return "000";

            return "";
        }

        private string GetComp(string comp)
        {
            if (comp.Equals(""))
                return "0101010";
            if (comp.Equals("0"))
                return "0101010";
            if (comp.Equals("1"))
                return "0111111";
            if (comp.Equals("-1"))
                return "0111010";
            if (comp.Equals("D"))
                return "0001100";
            if (comp.Equals("A"))
                return "0110000";
            if (comp.Equals("!D"))
                return "0001101";
            if (comp.Equals("!A"))
                return "0110001";
            if (comp.Equals("-D"))
                return "0001111";
            if (comp.Equals("-A"))
                return "0110011";
            if (comp.Equals("D+1"))
                return "0011111";
            if (comp.Equals("A+1"))
                return "0110111";
            if (comp.Equals("D-1"))
                return "0001110";
            if (comp.Equals("A-1"))
                return "0110010";
            if (comp.Equals("D+A"))
                return "0000010";
            if (comp.Equals("D-A"))
                return "0010011";
            if (comp.Equals("A-D"))
                return "0000111";
            if (comp.Equals("D&A"))
                return "0000000";
            if (comp.Equals("D|A"))
                return "0010101";

            if (comp.Equals("M"))
                return "1110000";
            if (comp.Equals("!M"))
                return "1110001";
            if (comp.Equals("-M"))
                return "1110011";
            if (comp.Equals("M+1"))
                return "1110111";

            if (comp.Equals("M-1"))
                return "1110010";
            if (comp.Equals("D+M"))
                return "1000010";
            if (comp.Equals("D-M"))
                return "1010011";
            if (comp.Equals("M-D"))
                return "1000111";
            if (comp.Equals("M-D"))
                return "1000111";
            if (comp.Equals("D&M"))
                return "1000000";
            if (comp.Equals("D|M"))
                return "1010101";

            return "";
        }

        private string GetDest(string dest)
        {
            if (dest.Equals(""))
                return "000";
            if (dest.Equals("0"))
                return "000";
            if (dest.Equals("M"))
                return "001";
            if (dest.Equals("D"))
                return "010";
            if (dest.Equals("MD"))
                return "011";
            if (dest.Equals("A"))
                return "100";
            if (dest.Equals("AM"))
                return "101";
            if (dest.Equals("AD"))
                return "110";
            if (dest.Equals("AMD"))
                return "111";

            return "";
        }

        private string GetBinary(string substring)
        {
            int.TryParse(substring, out int input);

            return Convert.ToString(input, 2);
        }
    }
}
