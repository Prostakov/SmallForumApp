using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace TestAuthorizationApp.Services
{
    public class EmailService
    {
        private readonly string _defaultEmailAddress;
        private readonly string _defaultEmailPassword;
        private readonly string _defaultEmailSmtpServer;
        private readonly int _defaultEmailSmtpPort;

        public EmailService(IOptions<Config> config)
        {
            _defaultEmailAddress = config.Value.DefaultEmail.Address;
            _defaultEmailPassword = config.Value.DefaultEmail.Password;
            _defaultEmailSmtpServer = config.Value.DefaultEmail.SmtpServer;
            _defaultEmailSmtpPort = Convert.ToInt32(config.Value.DefaultEmail.SmtpPort);
        }

        public async Task SendEmailAsync(string to, string toTitle, string subject, string body)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("TestAuthorizationApp", _defaultEmailAddress));
                mimeMessage.To.Add(new MailboxAddress(toTitle, to));
                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart("plain") { Text = body };

                using (var client = new SmtpClient())
                {
                    client.Connect(_defaultEmailSmtpServer, _defaultEmailSmtpPort, SecureSocketOptions.SslOnConnect);
                    client.Authenticate(_defaultEmailAddress, _defaultEmailPassword);
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                }
            }
            catch(Exception ex)
            {
                // ignored... for now...
                var a = 2;
            }
        }
    }
}
