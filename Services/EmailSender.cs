using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace XmlSigner.Services
{
    public class EmailSender : IEmailSender
    {
        private IConfiguration _configuration;
        public EmailSender(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        //https://code-maze.com/email-confirmation-aspnet-core-identity/
        //https://ethereal.email/create
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_configuration["SMTP:host"])    //_configuration["SMTP:port"]
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configuration["SMTP:user"], _configuration["SMTP:pass"])
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["SMTP:user"])
            };
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = htmlMessage;
            return client.SendMailAsync(mailMessage);
        }
    }
}
