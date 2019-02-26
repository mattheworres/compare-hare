#region usings

using System.Text;
using Microsoft.Extensions.Configuration;
using Stripe;

#endregion

namespace CompareHare.Api.AppStartup
{
    public static class StripeConfigurator
    {
        public static void Configure(IConfiguration configuration)
        {
            var apiKey = configuration["Stripe:ApiKey"];
            StripeConfiguration.SetApiKey(apiKey);
        }
    }
}
