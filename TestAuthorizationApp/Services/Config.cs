namespace TestAuthorizationApp.Services
{
    public class Config
    {
        public struct EmailInfo
        {
            public string Address { get; set; }
            public string Password { get; set; }
            public string SmtpServer { get; set; }
            public string SmtpPort { get; set; }
        }

        public EmailInfo DefaultEmail { get; set; }
    }
}
