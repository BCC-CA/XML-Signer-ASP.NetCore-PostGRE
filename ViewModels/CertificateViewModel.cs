using System;

namespace XmlSigner.ViewModels
{
    public class CertificateViewModel
    {
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public DateTime SigningTime { get; set; }
    }
}
