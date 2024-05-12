public class State
{
    IDictionary<string,NameSpace> Namespaces{get;set;}
    IDictionary<string,ClassMetaData> Classes{get; set;}
    IDictionary<string,InterfaceMetaData> Interfaces{get; set;}
}