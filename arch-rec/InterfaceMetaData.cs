
public class InterfaceMetaData: TypeMetaData
{
    public List<InterfaceMetaData> Implements { get; set; }

    public InterfaceMetaData(string name, NameSpace ns, List<InterfaceMetaData> implements): base(name, ns)
    {
        Implements = implements != null ? implements : new List<InterfaceMetaData>();
    }

    new internal void ResolveInheritance(Dictionary<string, ClassMetaData> classes, Dictionary<string, InterfaceMetaData> interfaces)
    {
        for (int i = UnresolvedImplInh.Count - 1; 0 <= i; i--)
        {
            var current = UnresolvedImplInh[i];

            if (interfaces.ContainsKey(current))
            {
                Implements.Add(interfaces[current]);
                UnresolvedImplInh.RemoveAt(i);
            }
        }
    }
}