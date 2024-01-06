using Microsoft.AspNetCore.Cors.Infrastructure;

namespace CompareHare.Api.AppStartup
{
    public static class CorsPolicyConfigurator
    {
        public static void Configure(CorsPolicyBuilder builder)
        {
            builder.AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials()
                   .WithOrigins("https://localhost:8000",
                        "http://localhost:8000");
        }
    }
}
