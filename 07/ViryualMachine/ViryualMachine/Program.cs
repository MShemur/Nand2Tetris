using System;
using System.IO;

namespace VirtualMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write file path: ");
            var input = Console.ReadLine();

            if (input != null && input[0] == '"') input = input.Substring(1);
            if (input != null && input[^1] == '"') input = input[..^1];

            if (!File.Exists(input) && !Directory.Exists(input))
            {
                Console.WriteLine("File or path does not exist");
                Console.ReadKey();
                return;
            }

            FileParser parser = new FileParser(input);
            var lines = parser.GetLines();

            CodeWriter codeWriter = new CodeWriter();
            codeWriter.WriteBootstrap();
            var commandParser = new CommandParser(lines);
            while (commandParser.HasMoreCommands)
            {
                commandParser.Advance();

                switch (commandParser.CommandTyp)
                {
                    case CommandType.Arithmetic:
                        codeWriter.WriteArithmetic(commandParser.Argument1);
                        break;
                    case CommandType.Push:
                    case CommandType.Pop:
                        codeWriter.WritePushPop(commandParser.CommandTyp, commandParser.Argument1, commandParser.Argument2);
                        break;
                    case CommandType.If:
                        codeWriter.WriteIf(commandParser.Argument1);
                        break;
                    case CommandType.Label:
                        codeWriter.WriteLabel(commandParser.Argument1);
                        break;
                    case CommandType.GoTo:
                        codeWriter.WriteGoTo(commandParser.Argument1);
                        break;
                    case CommandType.Function:
                        codeWriter.WriteFunction(commandParser.Argument1, commandParser.Argument2);
                        break;
                    case CommandType.Return:
                        codeWriter.WriteReturn();
                        break;
                    case CommandType.Call:
                        codeWriter.WriteCall(commandParser.Argument1, commandParser.Argument2);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var folderName = input.Substring(input.LastIndexOf(@"\", StringComparison.Ordinal) + 1);


            if (Directory.Exists(input))
                File.WriteAllLines(Path.ChangeExtension(input + "//" + folderName, "asm"), codeWriter.OutputLines);
            else
                File.WriteAllLines(Path.ChangeExtension(input, "asm"), codeWriter.OutputLines);

            Console.WriteLine("Finished writing");

            Console.ReadLine();
        }
    }
}
