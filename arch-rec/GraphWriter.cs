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

    private void WriteInterface(){

    }
}