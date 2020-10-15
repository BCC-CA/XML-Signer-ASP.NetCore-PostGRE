using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using XmlSigner.Data;
using XmlSigner.Data.Models;
using XmlSigner.Library;
using XmlSigner.Services;
using XmlSigner.Services.Models;

namespace XmlSigner.ViewModels
{
    public class ApplicationSignViewModel
    {
        private readonly IConfiguration _config;

        public XmlFile XmlFile { get; set; }
        public LeaveApplication LeaveApplication { get; set; }

        public bool isSignatureVerified { get; } = false;

        public List<CertificateModel> CertificateList { get; set; }

        [Obsolete]
        internal ApplicationSignViewModel(XmlFile xmlFile, IConfiguration configuration)
        {
            _config = configuration;
            XmlFile = xmlFile;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XmlFile.FileContent);
            LeaveApplication = Adapter.DeSerializeFromXml<LeaveApplication>(xmlDoc);
            bool verificationStatus = false;
            isSignatureVerified = verificationStatus;
        }

        [Obsolete]
        internal async Task<bool> VerifyXmlWithServiceAsync()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XmlFile.FileContent);
            XmlVerifierService xmlVerService = new XmlVerifierService(_config);
            CertificateList = await xmlVerService.GetAllSignedCertificateAsync(xmlDoc.OuterXml);
            return true;
        }

        internal async Task UpdateStatusFromDatabase(ApplicationDbContext DbContext)
        {
            LeaveApplication application = await DbContext.LeaveApplications.FindAsync(LeaveApplication.Id);
            LeaveApplication.ApplicationStatus = application.ApplicationStatus;
        }
    }
}
