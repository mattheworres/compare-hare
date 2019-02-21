#region usings

using CompareHare.Domain.Features.Interfaces;
using Microsoft.Extensions.Configuration;

#endregion

namespace CompareHare.Domain.Emails.Configuration
{
    public class SmtpConfiguration : IFeatureService
    {
        private readonly IConfiguration _configuration;

        public SmtpConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Host => _configuration["email:smtp:Host"];
        public int Port => _configuration.GetValue<int>("email:smtp:Port");
        public string Username => _configuration["email:smtp:Username"];
        public string Password => _configuration["email:smtp:Password"];
        public bool Secure => _configuration.GetValue<bool>("email:smtp:Secure");
    }
}
