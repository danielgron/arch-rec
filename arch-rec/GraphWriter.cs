public class GraphWriter
{
    public void WriteClasses(string outputPath){
    var sw = new StreamWriter(outputPath);


    using (sw)
        {
            sw.WriteLine("@startuml");
            sw.WriteLine("skinparam defaultFontSize 36");

            sw.WriteLine("@enduml");
        }
    }
}