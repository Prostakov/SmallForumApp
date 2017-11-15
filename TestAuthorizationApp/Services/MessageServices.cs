using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAuthorizationApp.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly EmailService _emailService;
        private readonly SmsService _smsService;

        public AuthMessageSender(EmailService emailService, SmsService smsService)
        {
            _emailService = emailService;
            _smsService = smsService;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return _emailService.SendEmailAsync(email, "", subject, message);
        }

        public Task SendSmsAsync(string number, string message)
        {
            return _smsService.SendSmsAsync(message, number);
        }
    }
}
