using System.IO;
using System.Xml.Serialization;

namespace Project.Helpers
{
    public class XmlSerialization
    {
        public static void SerializeObjectToXml<T>(T item, string filePath)
        {
            //remove namespaces from xml file
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var xs = new XmlSerializer(typeof(T), new XmlRootAttribute("Students"));
            using (var wr = new StreamWriter(filePath))
            {
                xs.Serialize(wr, item, ns);
            }
        }
    }
}