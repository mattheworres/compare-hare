#region usings

using Microsoft.AspNetCore.Cors.Infrastructure;

#endregion

namespace CompareHare.Api.AppStartup
{
    public static class CorsPolicyConfigurator
    {
        public static void Configure(CorsPolicyBuilder builder)
        {
            builder.AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials()
                   .WithOrigins("http://localhost:8000");
        }
    }
}
