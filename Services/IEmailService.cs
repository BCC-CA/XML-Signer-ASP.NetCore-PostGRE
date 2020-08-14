using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XmlSigner.Services
{
    interface IEmailService
    {
        void Send(string from, string to, string subject, string html);
    }
}
