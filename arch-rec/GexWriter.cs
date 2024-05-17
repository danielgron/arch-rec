public class GexfWriter
{


    public static void WriteClasses(string outputPath, Dictionary<string, ClassMetaData> classes, Dictionary<string, InterfaceMetaData> interfaces,
        List<string> unresolved)
    {
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
            Dictionary<string, int> weigths = new Dictionary<string, int>();

            foreach (var c in classes)
            {
                //if (c.Value.Implements.Count == 0) continue;
                if (c.Value.BaseClass != null)
                {

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

    public static void WriteNamespace(string outputPath, List<NameSpace> namespaces)
    {
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
                nodes.Add($"<node id=\"{n.FullName}\" label=\"{n.FullName}\"/>");
                //nodeIds.Add(n.Key, idCount);

                foreach (var u in n.Usings)
                {
                    var usingNameSpace = u.Value.NameSpace;
                    if (!namespaces.Any(n => n.FullName == usingNameSpace.FullName) && !addedDeps.ContainsKey(usingNameSpace.FullName))
                    {

                        nodes.Add($"<node id=\"{usingNameSpace.FullName}\" label=\"{usingNameSpace.FullName}\"/>");
                        nodeIds.Add(usingNameSpace.FullName, idCount);
                        addedDeps.Add(usingNameSpace.FullName, u.Value.NameSpace);

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
            var edges = new Dictionary<string, int>();

            foreach (var n in namespaces)
            {
                //if (c.Value.Implements.Count == 0) continue;
                foreach (var u in n.Usings)
                {
                    //var edge = $"<edge id=\"{edgeCount++}\" source=\"{nodeIds[n.Key]}.0\" target=\"{nodeIds[u.Value.FullName]}.0\" weight=\"<<w>>.0\"/>";
                    //var edge= new Edge(nodeIds[n.Key], nodeIds[u.Value.FullName],1);
                    if (!edges.ContainsKey($"{n.FullName}<>{u.Value.NameSpace.FullName}"))
                    {
                        
                        edges.Add($"{n.FullName}<>{u.Value.NameSpace.FullName}" ,u.Value.Count);
                    }
                    else{
                        edges[$"{n.FullName}<>{u.Value.NameSpace.FullName}"]++;
                    }
                }
            }
            foreach (var item in edges)
                {
                    var line = item.Key.Split("<>");
                    var edge = $"<edge id=\"{edgeCount++}\" source=\"{line[0]}\" target=\"{line[1]}\" weight=\"<<w>>.0\"/>";   
                    sw.WriteLine(edge.Replace("<<w>>",item.Value.ToString()));

                }
            sw.WriteLine("</edges>");
            sw.WriteLine("</graph>");
            sw.WriteLine("</gexf>");

        }
    }


    private void WriteInterface()
    {

    }
}