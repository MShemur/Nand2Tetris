using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VirtualMachine
{
    public class FileParser
    {
        private List<string> linesFromFile;

        public FileParser(string path)
        {
            if (Path.HasExtension(path))
                linesFromFile = File.ReadAllLines(path).ToList();
            else
                linesFromFile = GetLinesFromVMFiles(path);
        }

        private List<string> GetLinesFromVMFiles(string path)
        {
            var lines = new List<string>();

            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                if (Path.GetExtension(file) != ".vm") continue;

                lines.AddRange(File.ReadAllLines(file).ToList());
            }

            return lines;
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
                    StringBuilder aaaa = new StringBuilder();
                    while (aaa.Count > 0)
                    {
                        aaaa.Append(aaa[0] + " ");
                        aaa.RemoveAt(0);
                    }

                    normalLines.Add(aaaa.ToString().Substring(0, aaaa.Length - 1));
                }
            }

            return normalLines;
        }
    }
}
