using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using XmlSigner.Data.Models;
using XmlSigner.Library;

namespace XmlSigner.ViewModels
{
    public class ApplicationSignViewModel
    {
        public XmlFile XmlFile { get; set; }
        public LeaveApplication LeaveApplication { get; set; }
        public List<CertificateViewModel> CertificateList { get; set; }
        public ApplicationSignViewModel(XmlFile xmlFile)
        {
            this.XmlFile = xmlFile;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XmlFile.FileContent);
            LeaveApplication = Adapter.DeSerializeFromXml<LeaveApplication>(xmlDoc);
            CertificateList = GetModelFromcertificate(
                                    XmlSign.VerifyAllSign(xmlDoc)
                                );
        }

        private List<CertificateViewModel> GetModelFromcertificate(List<X509Certificate2> x509CertList)
        {
            List<CertificateViewModel> certView = new List<CertificateViewModel>();
            foreach(var x509cert in x509CertList)
            {
                certView.Add(GetCertificateViewModel(x509cert));
            }
            return certView;
        }

        private CertificateViewModel GetCertificateViewModel(X509Certificate2 x509cert)
        {
            CertificateViewModel cvm = new CertificateViewModel();
            cvm.Issuer = x509cert.Issuer;
            cvm.Subject = x509cert.Subject;
            cvm.ValidFrom = x509cert.NotBefore;
            cvm.ValidTo = x509cert.NotAfter;
            return cvm;
        }
    }
}
