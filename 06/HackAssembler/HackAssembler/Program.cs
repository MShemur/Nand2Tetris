using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write file path: ");
            var input = Console.ReadLine();

            if (input[0] == '"') input = input.Substring(1);
            if (input[input.Length - 1] == '"') input = input.Substring(0, input.Length - 1);

            Parser parser = new Parser(input);
            CodeGetter codeGetter = new CodeGetter(parser.GetLines());

            File.WriteAllLines(Path.ChangeExtension(input,"hack") , codeGetter.GetBinaryCommands());
            Console.WriteLine("Success");
            Console.ReadLine();
        }
    }
}
