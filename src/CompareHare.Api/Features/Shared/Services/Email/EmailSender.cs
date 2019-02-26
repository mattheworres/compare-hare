using System;
using System.Net;
using System.Net.Mail;
using CompareHare.Api.Features.Shared.Services.Email.Interfaces;
using CompareHare.Domain.Features.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CompareHare.Api.Features.Shared.Services.Email
{
    public class EmailSender : IEmailSender, IFeatureService
    {
        private EmailConfiguration _config;
        public EmailSender(IConfiguration configuration)
        {
            _config = configuration.GetSection("email").Get<EmailConfiguration>();
        }

        public void SendMailMessage(MailMessage email)
        {
            email.From = new MailAddress(_config.Smtp.From);

            try
            {
                using (var client = BuildClient())
                {
                    client.Send(email);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error sending email to recipients", string.Join(", ", email.From));
            }
        }

        private SmtpClient BuildClient()
        {
            var client = new SmtpClient();
            client.Host = _config.Smtp.Host;
            if (_config.Smtp.Port.HasValue)
            {
                client.Port = _config.Smtp.Port.Value;
            }
            client.EnableSsl = _config.Smtp.Secure;

            if (!string.IsNullOrEmpty(_config.Smtp.Username) && !string.IsNullOrEmpty(_config.Smtp.Password))
            {
                client.Credentials = new NetworkCredential(_config.Smtp.Username, _config.Smtp.Password);
            }

            return client;
        }
    }
}
