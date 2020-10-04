using System;
using System.Security.Cryptography.X509Certificates;

namespace XmlSigner.Library.Model
{
    public class Certificate
    {
        public Certificate(X509Certificate2 certificate, string timeString)
        {
            ValidFrom = certificate.NotBefore;
            ValidTo = certificate.NotAfter;
            Issuer = certificate.Issuer;
            Subject = certificate.Subject;
            SigningTime = (DateTime)Adapter.Base64DecodTime(timeString);
        }

        public DateTime ValidFrom { get; }
        public DateTime ValidTo { get; }
        public string Issuer { get; }
        public string Subject { get; }
        public DateTime SigningTime { get; }
    }
}
