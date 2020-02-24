using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XmlSigner.Library
{
    public static class Adapter
    {
        internal static async Task<string> ReadAsStringAsync(IFormFile file)
        {
            StringBuilder result = new StringBuilder();
            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }
            return result.ToString().Trim();
        }

        /*
        internal static XmlDocument SerializeToXmlOld<T>(T source)
        {
            XmlDocument document = new XmlDocument();
            XPathNavigator navigator = document.CreateNavigator();
            using (var writer = navigator.AppendChild())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, source);
            }
            return document;
        }
        */

        // https://stackoverflow.com/questions/2347642/deserialize-from-string-instead-textreader
        // Calling -> _ = Adapter.SerializeToXml(demoData).OuterXml;
        internal static XmlDocument SerializeToXml<T>(T objectEntry)
        {
            MemoryStream memoryStream = new MemoryStream();
            new XmlSerializer(typeof(T)).Serialize(new StreamWriter(memoryStream, Encoding.UTF8), objectEntry);

            XmlDocument document = new XmlDocument();
            document.LoadXml(Encoding.UTF8.GetString(memoryStream.ToArray()));
            return document;
        }

        // Calling -> Adapter.DeSerializeFromXml<DemoData>(xmlDocument);
        internal static T DeSerializeFromXml<T>(XmlDocument xmlDocument)
        {
            return (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(xmlDocument.OuterXml));
        }
    }
}
