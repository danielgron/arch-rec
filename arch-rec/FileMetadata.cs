using System.Data;
using System;
using System.Text.RegularExpressions;

public class FileMetadata
{
    public string Filename { get; set; }
    public string Classname { get; set; }
    public NameSpace NameSpace { get; set; }
    public IDictionary<string, NameSpace> Usings { get; set; }
    //public FileType fileType { get; set; }

    public Dictionary<string, ClassMetaData> Classes = new Dictionary<string, ClassMetaData>();
    public Dictionary<string, InterfaceMetaData> Interfaces = new Dictionary<string, InterfaceMetaData>();



    public FileMetadata(string filename, string[] codeLines, IDictionary<string, NameSpace> nameSpaces)
    {
        Usings = new Dictionary<string, NameSpace>();
        NameSpace ns = null;
        try
        {
            var usings = codeLines.Where(x => x.TrimStart().StartsWith("using"));

            foreach (var u in usings)
            {
                var fullusingNameSpace = u.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1].Replace(";", "");

                if (fullusingNameSpace.ToLower() == fullusingNameSpace)
                {
                    continue; //Hacky way of ignoring using statements that are not namespaces
                }
                var usingNameSpace = fullusingNameSpace.Split(".");
                for (int i = 0; i < usingNameSpace.Length; i++)
                {

                    if (Usings.ContainsKey(fullusingNameSpace))
                    {
                        continue;
                    }
                    var parentNS = i == 0 ? null : string.Join(".", usingNameSpace.Take(i + 1 - 1).ToArray());
                    var parent = i == 0 || !Usings.ContainsKey(parentNS) ? null : Usings[parentNS];
                    ns = new NameSpace(fullusingNameSpace, parent);
                    Usings.Add(ns.FullName, ns);
                }
            }

            var fullNameSpace = codeLines.First(x => x.TrimStart().StartsWith("namespace")).Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];
            var nameSpace = codeLines.First(x => x.TrimStart().StartsWith("namespace")).Split(" ", StringSplitOptions.RemoveEmptyEntries)[1].Split(".");
            if (fullNameSpace == "QuestPDF.Previewer")
            {
                Console.WriteLine("Found");
            }
            for (int i = 0; i < nameSpace.Length; i++)
            {
                var nsStrings = nameSpace.Take(i + 1).ToArray();
                var parentNS = i == 0 ? null : nameSpace.Take(i + 1 - 1).ToArray();
                var joined = i == 0 ? null : string.Join(".", parentNS);
                var parent = i == 0 ? null : nameSpaces[joined];

                var nsName = string.Join(".", nsStrings).Replace(";", "");
                if (nameSpaces.ContainsKey(nsName))
                {
                    ns = nameSpaces[nsName];
                    foreach (var u in Usings.Values)
                    {
                        ns.AddUsing(u);

                    }
                    continue;
                }
                ns = new NameSpace(nsName, parent);
                ns.Usings = Usings;
                nameSpaces.Add(ns.FullName, ns);
            }

            string pattern = @"\b(public|private|protected|internal|static)?\s*(class|interface)\s+(\w+)(\s*:\s*(\w+(\s*,\s*\w+)*))?";

            Regex regex = new Regex(pattern);

            for (int i = 0; i < codeLines.Length; i++)
            {
                var line = codeLines[i];
                if (line.Contains("interface") || line.Contains("class") || line.Contains("enum") || line.Contains("struct"))
                {
                    var brackets = 0;
                    var rBracketsTotal = 0;
                    var endLine = -1;

                    // Horribly formatted code could be an issue here
                    for (int j = i; j < codeLines.Length; j++)
                    {
                        var searchLine = codeLines[j];
                        var lBrackets = searchLine.Where(c => c == '{').Count();
                        var rBrackets = searchLine.Where(c => c == '}').Count();

                        rBracketsTotal += rBrackets;

                        brackets = brackets + lBrackets - rBrackets;

                        if (brackets == 0 && rBracketsTotal != 0)
                        {
                            endLine = j;
                            break;
                        }
                    }

                    // Find matches
                    MatchCollection matches = regex.Matches(line);

                    // Print all matched class or interface names along with their type
                    foreach (Match match in matches)
                    {
                        string type = match.Groups[2].Value; // Capture 'class' or 'interface'
                        string name = match.Groups[3].Value.Trim(); // Capture the name
                        string inheritance = match.Groups[5].Value;
                        string[] split = inheritance.Split(",", StringSplitOptions.RemoveEmptyEntries);

                        Console.WriteLine($"{line}");
                        Console.WriteLine($"Found {type}: {name}, Inheritance/Interfaces: {inheritance}");

                        if (name.ToLower() == name)
                        {
                            continue;
                        }


                        if (type == "class")
                        {

                            var c = new ClassMetaData(name, ns, null);
                            Classes.Add(c.Name, c);

                            for (int k = 0; k < split.Length; k++)
                            {
                                c.UnresolvedImplInh.Add(split[k].Trim());
                            }
                        }

                        if (type == "interface")
                        {

                            var iface = new InterfaceMetaData(name, ns, null);
                            Interfaces.Add(iface.Name, iface);
                            for (int k = 0; k < split.Length; k++)
                            {
                                iface.UnresolvedImplInh.Add(split[k]);
                            }
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to determine content of {filename}");
        }

    }
}