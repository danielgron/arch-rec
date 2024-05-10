using System.Data;

public class FileMetadata
{
    public string Filename { get; set; }
    public string Classname { get; set; }
    public NameSpace NameSpace { get; set; }
    public List<NameSpace> Usings { get; set; }


    public FileMetadata(string[] codeLines, IDictionary<string, NameSpace> nameSpaces)
    {
        var usings = codeLines.Where(x => x.TrimStart().StartsWith("using"));
        var nameSpace = codeLines.First(x => x.TrimStart().StartsWith("namespace")).Split(" ", StringSplitOptions.RemoveEmptyEntries)[1].Split(".");
              


    }
}