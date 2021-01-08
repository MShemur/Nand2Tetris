using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    class Parser
    {
        private readonly List<string> linesFromFile;


        public Parser(string path)
        {

            linesFromFile = File.ReadAllLines(path).ToList();
        }

        public List<string> GetLines()
        {
            return GetCommands(linesFromFile);
        }

        private List<string> GetCommands(List<string> lines)
        {
            List<string> normalLines = new List<string>();
            foreach (var line in lines)
            {
                int index = line.Length;
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line.Contains(@"//"))
                    index = line.IndexOf("//", StringComparison.Ordinal);

                var substring = line.Substring(0, index);

                if (!string.IsNullOrWhiteSpace(substring))
                {
                    var aaa = substring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    StringBuilder asaa = new StringBuilder();
                    while (aaa.Count > 0)
                    {
                        asaa.Append(aaa[0]);
                        aaa.RemoveAt(0);
                    }
                    normalLines.Add(asaa.ToString());
                }
            }

            return normalLines;
        }
    }
}
