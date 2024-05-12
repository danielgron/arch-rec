using System.Data;
using System;
using System.Text.RegularExpressions;

public class FileMetadata
{
    public string Filename { get; set; }
    public string Classname { get; set; }
    public NameSpace NameSpace { get; set; }
    public List<NameSpace> Usings { get; set; }
    public FileType fileType { get; set; }


    public FileMetadata(string filename, string[] codeLines, IDictionary<string, NameSpace> nameSpaces)
    {
        try
        {
            var usings = codeLines.Where(x => x.TrimStart().StartsWith("using"));
            var nameSpace = codeLines.First(x => x.TrimStart().StartsWith("namespace")).Split(" ", StringSplitOptions.RemoveEmptyEntries)[1].Split(".");

            for (int i = 0; i < nameSpace.Length; i++)
            {
                if (nameSpaces.ContainsKey(nameSpace[i]))
                {
                    continue;
                }
                var parent = i == 0 ? null : nameSpaces[nameSpace[i - 1]];
                var ns = new NameSpace(nameSpace[i], parent);
                nameSpaces.Add(ns.Name, ns);
            }



            var classInterface = codeLines.Where(x => x.Contains("interface") || x.Contains("class") || x.Contains("enum") || x.Contains("struct"));
            string pattern = @"\b(public|private|protected|internal|static)?\s*(class|interface)\s+(\w+)(\s*:\s*(\w+(\s*,\s*\w+)*))?";


            Regex regex = new Regex(pattern);

            foreach (var item in classInterface)
            {
                // Find matches
                MatchCollection matches = regex.Matches(item);

                // Print all matched class or interface names along with their type
                foreach (Match match in matches)
                {
                    string type = match.Groups[2].Value; // Capture 'class' or 'interface'
                    string name = match.Groups[3].Value; // Capture the name
                    string inheritance = match.Groups[5].Value;
                    Console.WriteLine($"{item}");
                    Console.WriteLine($"Found {type}: {name}, Inheritance/Interfaces: {inheritance}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to determine content of {filename}");
        }

    }
}