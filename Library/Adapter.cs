using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
    }
}
