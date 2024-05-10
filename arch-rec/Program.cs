// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var path = args[0];

var csFiles = Directory.GetFiles(path).Where(f => f.EndsWith(".cs"));

foreach (var file in csFiles)
{
    string[] fileContent = File.ReadAllLines(path);
    var usings = fileContent.Where(x => x.Trim().StartsWith("using"));

    



}