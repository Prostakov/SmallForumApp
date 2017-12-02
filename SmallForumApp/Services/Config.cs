namespace SmallForumApp.Services
{
    public class Config
    {
        public struct DefaultUsersInfo
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

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

        public DefaultUsersInfo DefaultUsers { get; set; }
    }
}
