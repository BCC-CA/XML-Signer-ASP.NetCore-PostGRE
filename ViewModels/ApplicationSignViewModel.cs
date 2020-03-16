using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using XmlSigner.Data;
using XmlSigner.Data.Models;
using XmlSigner.Library;
using XmlSigner.Library.Model;

namespace XmlSigner.ViewModels
{
    public class ApplicationSignViewModel
    {
        public XmlFile XmlFile { get; set; }
        public LeaveApplication LeaveApplication { get; set; }
        public List<Certificate> CertificateList { get; set; }

        public ApplicationSignViewModel(XmlFile xmlFile)
        {
            XmlFile = xmlFile;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XmlFile.FileContent);
            LeaveApplication = Adapter.DeSerializeFromXml<LeaveApplication>(xmlDoc);
            CertificateList = XmlSign.GetAllSign(xmlDoc);
        }

        internal async Task UpdateStatusFromDatabase(ApplicationDbContext DbContext)
        {
            LeaveApplication application = await DbContext.LeaveApplications.FindAsync(LeaveApplication.Id);
            LeaveApplication.ApplicationStatus = application.ApplicationStatus;
        }
    }
}
