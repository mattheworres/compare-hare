#region usings

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace CompareHare.Api.AppStartup
{
    public static class JwtBearerConfigurator
    {
        public static void Configure(IConfiguration configuration, JwtBearerOptions options)
        {
            var keyByteArray = Encoding.UTF8.GetBytes(configuration["Auth:jwt:SigningKey"]);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidIssuer = configuration["Auth:jwt:Issuer"],
                    ValidAudience = configuration["Auth:jwt:Audience"],
                    IssuerSigningKey = signingKey
                };
        }
    }
}
