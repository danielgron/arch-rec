using System.Collections.Generic;


public class NameSpace
{
    public NameSpace Parent { get; set; }
    public IDictionary<string, NameSpace> Children { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public List<FileMetadata> Files {get; private set;}
    public IDictionary<string, NamespaceWrapper> Usings { get; set; }

    public NameSpace(string fullName, NameSpace parent)
    {
        FullName = fullName;
        Name = fullName.Split(".")[^1];
        Parent = parent;
        if (Parent != null) Parent.AddChild(this);
        Files = new List<FileMetadata>();
        Children = new Dictionary<string, NameSpace>();
        Usings = new Dictionary<string, NamespaceWrapper>();
    }

    public void AddFile(FileMetadata file){
        Files.Add(file);
    }

    public void AddUsing(NameSpace ns){
        if(Usings.ContainsKey(ns.FullName)){
            Usings[ns.FullName].Count++;
        }
        else Usings.Add(ns.FullName, new NamespaceWrapper(ns));
    }

    public void AddChild(NameSpace child){
        Children.Add(child.Name, child);
    }

    public override string ToString(){
        if (Parent != null) return Parent.Name + "." + Name;
        return Name;
    }
}