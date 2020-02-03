using Microsoft.AspNetCore.Http;
using System;
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
            return result.ToString();
        }

        internal static async Task<MemoryStream> ReadAsMemoryStreamAsync(string fileContent)
        {
            /*using (MemoryStream memStream = new MemoryStream())
            { }*/
            MemoryStream memoryStream = new MemoryStream();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(fileContent);
            try
            {
                await memoryStream.WriteAsync(data, 0, data.Length);
                return memoryStream;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
