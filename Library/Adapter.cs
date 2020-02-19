using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

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

        internal static XmlDocument SerializeToXml<T>(T source)
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
    }
}
