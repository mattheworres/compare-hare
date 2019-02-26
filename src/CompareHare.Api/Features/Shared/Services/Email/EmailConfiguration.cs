namespace CompareHare.Api.Features.Shared.Services.Email
{
    public class EmailConfiguration
    {
        public SmtpConfiguration Smtp { get; set; }
    }

    public class SmtpConfiguration
    {
        public string From { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Secure { get; set; }
    }
}
