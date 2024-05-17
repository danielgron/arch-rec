// See https://aka.ms/new-console-template for more information

var path = args[0];

var csFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Where(f => !Path.GetDirectoryName(f).Contains("Test") && f.EndsWith(".cs"));
var csprojFiles = Directory.GetFiles(path).Where(f => f.EndsWith(".csproj"));

var classes = new Dictionary<string, ClassMetaData>();
var interfaces = new Dictionary<string, InterfaceMetaData>();
var unresolved = new List<string>();
var ns = new List<NameSpace>();

foreach (var file in csFiles)
{
    string[] fileContent = File.ReadAllLines(file);
    var testFile = false;
    foreach (var line in fileContent)
    {
        if (line.Contains("[Test]")) testFile = true;
        
    }
    if (testFile) continue;

    var fmd = new FileMetadata(file, fileContent, ns);


    foreach (var fmdClass in fmd.Classes)
    {
        if (classes.ContainsKey(fmdClass.Key))
        {
            Console.WriteLine($"Duplicate class {fmdClass.Key} found in {file}");
            continue;
        }
        classes.Add(fmdClass.Key, fmdClass.Value);
    }
    foreach (var fmdInterface in fmd.Interfaces)
    {
        if (interfaces.ContainsKey(fmdInterface.Key))
        {
            Console.WriteLine($"Duplicate interface {fmdInterface.Key} found in {file}");
            continue;
        }
        interfaces.Add(fmdInterface.Key, fmdInterface.Value);
    }

}

foreach (var c in classes)
{
    c.Value.ResolveInheritance(classes, interfaces);
    // add unresolved to unresolved list, but only if it's not already in there
    foreach (var item in c.Value.UnresolvedImplInh)
    {
        if (!unresolved.Contains(item))
        {
            unresolved.Add(item);
        }
    }
}

foreach (var iface in interfaces)
{
    iface.Value.ResolveInheritance(classes, interfaces);
    foreach (var item in iface.Value.UnresolvedImplInh)
    {
        if (!unresolved.Contains(item))
        {
            unresolved.Add(item);
        }
    }
}

GraphWriter.WriteClasses("./cidiagram.puml", classes, interfaces, unresolved);
GexfWriter.WriteClasses("./classdiagram.gexf", classes, interfaces, unresolved);
//GraphWriter.WriteNamespace("./nsdiagram.puml", ns);
GexfWriter.WriteNamespace("./nsdiagram.gexf", ns);

foreach (var n in ns)
{
    //Console.WriteLine(n.Value);


}