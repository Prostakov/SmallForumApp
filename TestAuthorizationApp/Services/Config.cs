namespace TestAuthorizationApp.Services
{
    public class Config
    {
        public struct EmailServiceInfo
        {
            public string SmtpServer { get; set; }
            public string SmtpPort { get; set; }
            public string SenderEmailAddress { get; set; }
            public string SenderEmailPassword { get; set; }
        }

        public struct SmsServiceInfo
        {
            public string ServiceUri { get; set; }
            public string SenderLogin { get; set; }
            public string SenderPassword { get; set; }
            public string SmsAlphaName { get; set; }
        }

        public EmailServiceInfo EmailService { get; set; }

        public SmsServiceInfo SmsService { get; set; }
    }
}
