// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var path = args[0];

var csFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Where(f => f.EndsWith(".cs"));
var csprojFiles = Directory.GetFiles(path).Where(f => f.EndsWith(".csproj"));

var classes = new Dictionary<string, FileMetadata>();
var ns = new Dictionary<string, NameSpace>();

foreach (var file in csFiles)
{
    string[] fileContent = File.ReadAllLines(file);
    var fmd = new FileMetadata(file, fileContent, ns);
    
}

foreach (var n in ns)
{
    Console.WriteLine(n.Value);
}