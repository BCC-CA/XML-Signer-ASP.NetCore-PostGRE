using System.Threading.Tasks;
using XmlSigner.Data.Models;
using XmlSigner.Library;

namespace XmlSigner.ViewModels.Mappers
{
    public class XmlFileMapper
    {
        //Should implement manual mapper
        internal static async Task<XmlFile> MapFromXmlFileAddViewModelAsync(XmlFileAddViewModel xmlFileAddViewModel)
        {
            XmlFile xmlFile = new XmlFile();
            if (xmlFileAddViewModel.PreviousFileId != null)
            {
                xmlFile.PreviousFileId = xmlFileAddViewModel.PreviousFileId;
            }
            if (xmlFileAddViewModel.XmlFile.Length > 0)
            {
                xmlFile.FileContent = await Adapter.ReadAsStringAsync(xmlFileAddViewModel.XmlFile);
                xmlFile.FileRealName = xmlFileAddViewModel.XmlFile.FileName;
                return xmlFile;
            }
            else
            {
                return null;
            }
        }
    }
}
