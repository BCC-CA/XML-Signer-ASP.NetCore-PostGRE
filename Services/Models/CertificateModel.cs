using System;

namespace XmlSigner.Services.Models
{
    public class CertificateModel
    {
        public DateTime CertificateValidFrom { get; set; }
        public DateTime CertificateValidTo { get; set; }
        public string CertificateIssuer { get; set; }
        public string CertificateSubject { get; set; }
        public string CertificateHash { get; set; }
        public DateTime SigningTime { get; set; }
        public string tsaSignedTimestamp_Base64_UTF8 { get; set; }
    }
}
