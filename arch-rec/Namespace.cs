using System.Collections.Generic;

public class NameSpace
{
    public NameSpace Parent { get; set; }
    public IDictionary<string, NameSpace> Children { get; set; }
    public string Name { get; set; }
    public List<FileMetadata> Files {get;set;}

    public NameSpace(string name, NameSpace parent)
    {
        Name = name;
        Parent = parent;
        if (Parent != null) Parent.AddChild(this);
        Files = new List<FileMetadata>();
        Children = new Dictionary<string, NameSpace>();
    }

    public void AddChild(NameSpace child){
        Children.Add(child.Name, child);
    }

    public override string ToString(){
        if (Parent != null) return Parent.Name + "." + Name;
        return Name;
    }
}