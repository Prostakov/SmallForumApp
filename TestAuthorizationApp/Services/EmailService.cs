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
        private readonly string _emailSenderAddress;
        private readonly string _emailSenderPassword;
        private readonly string _smtpServer;
        private readonly int _smtpPort;

        public EmailService(IOptions<Config> config)
        {
            _emailSenderAddress = config.Value.EmailService.SenderEmailAddress;
            _emailSenderPassword = config.Value.EmailService.SenderEmailPassword;
            _smtpServer = config.Value.EmailService.SmtpServer;
            _smtpPort = Convert.ToInt32(config.Value.EmailService.SmtpPort);
        }

        public async Task SendEmailAsync(string to, string toTitle, string subject, string body)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("TestAuthorizationApp", _emailSenderAddress));
                mimeMessage.To.Add(new MailboxAddress(toTitle, to));
                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart("plain") { Text = body };

                using (var client = new SmtpClient())
                {
                    client.Connect(_smtpServer, _smtpPort, SecureSocketOptions.SslOnConnect);
                    client.Authenticate(_emailSenderAddress, _emailSenderPassword);
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                }
            }
            catch
            {
                // ignored... for now...
            }
        }
    }
}
