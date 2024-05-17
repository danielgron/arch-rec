public class GraphWriter
{

    
    public static void WriteClasses(string outputPath, Dictionary<string, ClassMetaData> classes, Dictionary<string, InterfaceMetaData> interfaces, 
        List<string> unresolved){
    var sw = new StreamWriter(outputPath);


    using (sw)
        {
            sw.WriteLine("@startuml");
            sw.WriteLine("skinparam defaultFontSize 16");
            sw.WriteLine($"!define InterfaceMD circle #green");
            sw.WriteLine($"!define ClassMD circle #black");
            sw.WriteLine($"!define ExternalMD circle #red");

            sw.WriteLine("ExternalMD {");
            sw.WriteLine("FontSize 30");
            sw.WriteLine("BackGroundColor yellow");
            sw.WriteLine("Margin 30");
            sw.WriteLine("Padding 50");
            sw.WriteLine("}");
                    foreach (var c in classes)
            {
                //if (c.Value.Implements.Count == 0) continue;
                sw.WriteLine($"ClassMD {c.Key}");
            }
            foreach (var i in interfaces)
            {
                sw.WriteLine($"InterfaceMD {i.Key}");
            }

            foreach (var u in unresolved)
            {
                sw.WriteLine($"ExternalMD {u}");
            }

            foreach (var c in classes)
            {
                //if (c.Value.Implements.Count == 0) continue;
                if (c.Value.BaseClass != null){
                    sw.WriteLine($"{c.Key}  --> {c.Value.BaseClass.Name}");
                }
                foreach (var iface in c.Value.Implements)
                {
                    sw.WriteLine($"{c.Key} --> {iface.Name}");
                }
                
            }

            sw.WriteLine("@enduml");
        }


    }

    public static void WriteNamespace(string outputPath, Dictionary<string, NameSpace> namespaces){
        var sw = new StreamWriter(outputPath);


    Dictionary<string, NameSpace> addedDeps = new Dictionary<string, NameSpace>();

    using (sw)
        {
            sw.WriteLine("@startuml");
            sw.WriteLine("left to right direction");
            sw.WriteLine("skinparam defaultFontSize 16");
            sw.WriteLine($"!define InternalMD circle #green");
            sw.WriteLine($"!define ExternalMD circle #red");

            sw.WriteLine("ExternalMD {");
            sw.WriteLine("FontSize 30");
            sw.WriteLine("BackGroundColor yellow");
            sw.WriteLine("Margin 30");
            sw.WriteLine("Padding 50");
            sw.WriteLine("}");

            var addedNamespaces = new Dictionary<string, NameSpace>();

            foreach (var n in namespaces)
            {
                sw.WriteLine($"InternalMD {n.Key}");
                foreach (var u in n.Value.Usings)
                {
                    if (!namespaces.ContainsKey(u.Value.NameSpace.FullName) && !addedDeps.ContainsKey(u.Value.NameSpace.FullName)){
                        sw.WriteLine($"ExternalMD {u.Value.NameSpace.FullName}");
                        addedDeps.Add(u.Value.NameSpace.FullName, u.Value.NameSpace);

                    }
                }
            }

            foreach (var n in namespaces)
            {
                //if (c.Value.Implements.Count == 0) continue;
                foreach (var u in n.Value.Usings)
                {
                    //if (namespaces.ContainsKey(u.Value.FullName)){
                        sw.WriteLine($"{n.Key}  --> {u.Value.NameSpace.FullName}");
                    //}
                }
                
            }

            sw.WriteLine("@enduml");
        }
    }


    private void WriteInterface(){

    }
}