using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using XmlSigner.Services.Models;

namespace XmlSigner.Services
{
    public class XmlVerifierService
    {
        private string ApiUrl;
        internal bool IsSignatureVerified = false;
        public List<CertificateModel> CertificateList { get; private set; }

        public XmlVerifierService(IConfiguration configuration)
        {
            ApiUrl = configuration.GetSection("XmlVerifierServiceUrl").Value;
        }

        [Obsolete]
        internal async Task<List<CertificateModel>> GetAllSignedCertificateAsync(string xmlString)
        {
            List<CertificateModel> certList = await GetVerificationResponseAsync(xmlString);
            CertificateList = certList;
            return certList;
        }

        [Obsolete]
        private async Task<List<CertificateModel>> GetVerificationResponseAsync(string xmlString)
        {
            RestClient client = new RestClient(ApiUrl);
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;    //Disable Certificate Check
            RestRequest uploadRequest = new RestRequest("", Method.POST);
            uploadRequest.AddParameter("xml", xmlString);
            IRestResponse uploadResponse = await client.ExecutePostTaskAsync(uploadRequest);
            if (uploadResponse.StatusCode.CompareTo(HttpStatusCode.OK) == 0)
            {
                JObject jObject = JObject.Parse( uploadResponse.Content );
                IsSignatureVerified = (bool)jObject["success"];
                List<CertificateModel> certList = new List<CertificateModel>();
                if (IsSignatureVerified==true)
                {
                    foreach(JToken signature in jObject["signatures"])
                    {
                        CertificateModel certModel = new CertificateModel();
                        certModel.CertificateValidFrom = (DateTime)signature["certificateValidFrom"];
                        certModel.CertificateValidTo = (DateTime)signature["certificateValidFrom"];
                        certModel.CertificateHash = (string)signature["certificateHash"];
                        certModel.CertificateIssuer = (string)signature["certificateIssuer"];
                        certModel.CertificateSubject = (string)signature["certificateSubject"];
                        certModel.SigningTime = (DateTime)signature["signingTime"];
                        certModel.tsaSignedTimestamp_Base64_UTF8 = (string)signature["tsaSignedTimestamp_Base64_UTF8"];
                        certList.Add(certModel);
                    }
                }
                return certList;
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }
    }
}
