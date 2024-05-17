public class GexfWriter
{

    
    public static void WriteClasses(string outputPath, Dictionary<string, ClassMetaData> classes, Dictionary<string, InterfaceMetaData> interfaces, 
        List<string> unresolved){
    var sw = new StreamWriter(outputPath);


    var nodeCount = classes.Count + interfaces.Count + unresolved.Count;
    var idCount = 0;
    var edgeCount = 0;
    var nodeIds = new Dictionary<string, int>();



    using (sw)
        {
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sw.WriteLine("<gexf xmlns:viz=\"http:///www.gexf.net/1.1draft/viz\" version=\"1.1\" xmlns=\"http://www.gexf.net/1.1draft\">");
            sw.WriteLine($"<graph defaultedgetype=\"directed\" idtype=\"string\" type=\"static\">");
            sw.WriteLine("<attributes class=\"node\" mode=\"static\">");
            sw.WriteLine("<attribute id=\"0\" title=\"Type\" type=\"string\"/>");
            sw.WriteLine("</attributes>");
            sw.WriteLine($"<nodes count=\"{nodeCount}\">");
            
                    foreach (var c in classes)
            {
                //if (c.Value.Implements.Count == 0) continue;
                sw.WriteLine($"<node id=\"{idCount++}.0\" label=\"{c.Key}\"/>");
                sw.WriteLine("<attvalues>");
                sw.WriteLine($"<attvalue for=\"0\" value=\"Class\"/>");
                sw.WriteLine("</attvalues>"); 
                nodeIds.Add(c.Key, idCount);
            }
            foreach (var i in interfaces)
            {
                sw.WriteLine($"<node id=\"{idCount++}.0\" label=\"{i.Key}\" type=\"Interface\"/>");
                nodeIds.Add(i.Key, idCount);
            }

            foreach (var u in unresolved)
            {
                sw.WriteLine($"<node id=\"{idCount++}.0\" label=\"{u}\" type=\"Unresolved\"/>");
                nodeIds.Add(u, idCount);
            }

            List<string> edges = new List<string>();

            foreach (var c in classes)
            {
                //if (c.Value.Implements.Count == 0) continue;
                if (c.Value.BaseClass != null){
                    edges.Add($"<edge id=\"{edgeCount++}\" source=\"{nodeIds[c.Key]}.0\" target=\"{nodeIds[c.Value.BaseClass.Name]}.0\"/>");
                    
                }
                foreach (var iface in c.Value.Implements)
                {
                    edges.Add($"<edge id=\"{edgeCount++}\" source=\"{nodeIds[c.Key]}.0\" target=\"{nodeIds[iface.Name]}.0\"/>");
                    
                }
                
            }
            sw.WriteLine("</nodes>");
            sw.WriteLine($"<edges count=\"{edgeCount}\">");
            foreach (var e in edges)
            {
                sw.WriteLine(e);
            }
            sw.WriteLine("</edges>");
            sw.WriteLine("</graph>");
            sw.WriteLine("</gexf>");
        
        }


    }

    public static void WriteNamespace(string outputPath, Dictionary<string, NameSpace> namespaces){
        var sw = new StreamWriter(outputPath);


    Dictionary<string, NameSpace> addedDeps = new Dictionary<string, NameSpace>();

    var nodeCount = 0;
    var idCount = 0;
    var edgeCount = 0;
    var nodeIds = new Dictionary<string, int>();

    using (sw)
        {
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sw.WriteLine("<gexf xmlns:viz=\"http:///www.gexf.net/1.1draft/viz\" version=\"1.1\" xmlns=\"http://www.gexf.net/1.1draft\">");
            sw.WriteLine($"<graph defaultedgetype=\"directed\" idtype=\"string\" type=\"static\">");

            List<string> nodes = new List<string>();

            var addedNamespaces = new Dictionary<string, NameSpace>();

            foreach (var n in namespaces)
            {
                nodes.Add($"<node id=\"{idCount++}.0\" label=\"{n.Key}\"/>");
                nodeIds.Add(n.Key, idCount);

                foreach (var u in n.Value.Usings)
                {
                    if (!namespaces.ContainsKey(u.Value.FullName) && !addedDeps.ContainsKey(u.Value.FullName)){

                        nodes.Add($"<node id=\"{idCount++}.0\" label=\"{u.Value.FullName}\"/>");
                        nodeIds.Add(u.Value.FullName, idCount);
                        addedDeps.Add(u.Value.FullName, u.Value);

                    }
                }
            }
            sw.WriteLine($"<nodes count=\"{nodes.Count}\">");
            foreach (var item in nodes)
            {
                sw.WriteLine(item);
            }
            sw.WriteLine("</nodes>");
            sw.WriteLine("<edges>");
            var edges = new List<string>();

            foreach (var n in namespaces)
            {
                //if (c.Value.Implements.Count == 0) continue;
                foreach (var u in n.Value.Usings)
                {
                    //if (namespaces.ContainsKey(u.Value.FullName)){
                        //sw.WriteLine($"{n.Key}  --> {u.Value.FullName}");
                        edges.Add($"<edge id=\"{edgeCount++}\" source=\"{nodeIds[n.Key]}.0\" target=\"{nodeIds[u.Value.FullName]}.0\"/>");

                    //}
                }
                foreach (var item in edges)
                {
                    sw.WriteLine(item);
                    
                }
                
            }
            sw.WriteLine("</edges>");
            sw.WriteLine("</graph>");
            sw.WriteLine("</gexf>");

        }
    }


    private void WriteInterface(){

    }
}