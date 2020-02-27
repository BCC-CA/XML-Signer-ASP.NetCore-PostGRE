using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace XmlSigner.Library
{
    class XmlSign
    {
        internal static bool CheckIfDocumentPreviouslySigned(XmlDocument xmlDocument)
        {
            int signCount = DocumentSignCount(xmlDocument);
            if (signCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static int DocumentSignCount(XmlDocument xmlDocument)
        {
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");
            return nodeList.Count;
        }

        internal static List<X509Certificate2> VerifyAllSign(XmlDocument xmlDocument)
        {
            if (!CheckIfDocumentPreviouslySigned(xmlDocument))
                return null;    //File has no sign
            List<X509Certificate2> signerCertificateList = new List<X509Certificate2>();

            while (CheckIfDocumentPreviouslySigned(xmlDocument))
            {
                if (VerifyLastSign(xmlDocument) == false)
                {
                    return null;   //Not counting all sign, find first invalid sign and tell that file is invalid
                }
                else
                {
                    signerCertificateList.Add(GetLastSignerCertificate(xmlDocument));
                    //Extract last sign key and get into X509Certificate2 - signerCertificateList
                }
                //Update xmlDocument by removing last sign tag
                xmlDocument = RemoveLastSign(xmlDocument);
            }
            return signerCertificateList;
        }

        private static XmlDocument RemoveLastSign(XmlDocument xmlDocument)
        {
            //nodes[i].ParentNode.RemoveChild(nodes[i]);
            XmlNodeList signList = xmlDocument.GetElementsByTagName("Signature");
            int indexToRemove = signList.Count - 1;
            signList[indexToRemove].ParentNode.RemoveChild(signList[indexToRemove]);
            return xmlDocument;
        }

        private static bool? VerifyLastSign(XmlDocument xmlDocument)
        {
            if (!CheckIfDocumentPreviouslySigned(xmlDocument))
            {
                return null;    //File has no sign
            }
            if (VerifySignedXmlLastSignWithoutCertificateVerification(xmlDocument))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Should get data from XmlDocument, not file
        private static bool VerifySignedXmlLastSignWithoutCertificateVerification(XmlDocument xmlDocument)
        {
            try
            {
                // Create a new SignedXml object and pass it
                SignedXml signedXml = new SignedXml(xmlDocument);

                // Find the "Signature" node and create a new
                // XmlNodeList object.
                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

                // Load the signature node.
                signedXml.LoadXml((XmlElement)nodeList[nodeList.Count - 1]);

                //////////////////////////////////Extract key - Start
                //X509Certificate2 x509 = GetLastSignerCertificate(xmlDocument);
                //////////////////////////////////Extract key - End

                AsymmetricAlgorithm key;
                bool signatureCheckStatus = signedXml.CheckSignatureReturningKey(out key);
                if (signatureCheckStatus)
                {
                    XmlElement metaElement = (XmlElement)nodeList[nodeList.Count - 1].LastChild;
                    //return VerifyMetaDataObjectSignature(metaElement, key);
                    return true;
                }
                else
                {
                    return false;
                }
                //return signedXml.CheckSignature(key);
                //return signedXml.CheckSignature(certificate, true);
            }
            catch (Exception exception)
            {
                Console.Write("Error: " + exception);
                throw exception;
            }
        }

        private static X509Certificate2 GetLastSignerCertificate(XmlDocument xmlDocument)
        {
            if (!CheckIfDocumentPreviouslySigned(xmlDocument))
            {
                return null;
            }
            XmlDocument document = new XmlDocument();

            // Find the "Signature" node and create a new
            // XmlNodeList object.
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

            // Load the signature node.

            document.LoadXml(((XmlElement)nodeList[nodeList.Count - 1]).OuterXml);
            string certString = document.GetElementsByTagName("X509Data")[0].InnerText;
            /*...Decode text in cert here (may need to use Encoding, Base64, UrlEncode, etc) ending with 'data' being a byte array...*/
            return new X509Certificate2(Encoding.ASCII.GetBytes(certString));
        }
    }
}
