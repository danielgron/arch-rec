// Create a super class for all the metadata classes

using System.Xml.Serialization;

public class TypeMetaData
{
    public string Name { get; set; }
    public NameSpace NameSpace { get; set; }
    public List<NameSpace> Usings { get; private set; } = new();
    public List<string> UnresolvedImplInh { get; set; } = new();

    public TypeMetaData(string name, NameSpace nameSpace)
    {
        Name = name;
        NameSpace = nameSpace;
    }

    internal void ResolveInheritance(Dictionary<string, ClassMetaData> classes, Dictionary<string, InterfaceMetaData> interfaces)
    {
        foreach (var unres in UnresolvedImplInh)
        {
            // check if unres is in classes, if so add as base class and remove from unresolved
            if (classes.ContainsKey(unres))
            {
                ResolveInheritance(classes, interfaces);
                UnresolvedImplInh.Remove(unres);
            }
            // check if unres is in interfaces, if so add as interface and remove from unresolved
            else if (interfaces.ContainsKey(unres))
            {
                ResolveInheritance(classes, interfaces);
                UnresolvedImplInh.Remove(unres);
            }
        }
    }

    internal void ResolveUsings(){
        

    }
}