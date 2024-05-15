
public class ClassMetaData : TypeMetaData{
    public ClassMetaData? BaseClass {get;set;}
    public List<InterfaceMetaData> Implements {get;set;}

    public ClassMetaData(string name, NameSpace nameSpace, List<InterfaceMetaData> implements): base(name, nameSpace)
    {
        Name = name;
        NameSpace = nameSpace;
        Implements = implements != null ? implements : new List<InterfaceMetaData>();
    }

    new internal void ResolveInheritance(Dictionary<string, ClassMetaData> classes, Dictionary<string, InterfaceMetaData> interfaces)
    {
        
        for (int i = UnresolvedImplInh.Count - 1; 0 <= i; i--)
        {
            var current = UnresolvedImplInh[i];

            if (classes.ContainsKey(current))
            {
                BaseClass = classes[current];
                UnresolvedImplInh.RemoveAt(i);
            }
            else if (interfaces.ContainsKey(current))
            {
                Implements.Add(interfaces[current]);
                UnresolvedImplInh.RemoveAt(i);
            }
        }
    }
}