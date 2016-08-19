using System;
using System.Configuration;
using System.Net.Mail;
using AcklenAvenue.Email;
using Typesafe.Mailgun;

namespace Ironhide.EmailClients.DotNet
{
    public class MailGunSmtpClient : ISmtpClient
    {
        readonly MailgunClient _client;

        public MailGunSmtpClient()
        {
            var domain = ConfigurationManager.AppSettings["Mailgun_Domain"];
            var key = ConfigurationManager.AppSettings["Mailgun_Key"];
            var version = ConfigurationManager.AppSettings["Mailgun_API_Version"];
            _client = new MailgunClient(domain, key, Convert.ToInt32(version));
        }

        public void Send(string emailAddress, string subject, string body)
        {
            string from = ConfigurationManager.AppSettings["email_from"];
            var message = new MailMessage(from, emailAddress, subject, body);
            message.IsBodyHtml = false;
            _client.SendMail(message);
        }
    }
}