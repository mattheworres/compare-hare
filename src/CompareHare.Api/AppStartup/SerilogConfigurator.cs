#region usings

using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;

#endregion

namespace CompareHare.Api.AppStartup
{
    public class SerilogConfigurator
    {
        public static void Configure()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .Build();

            // Log.Logger = new LoggerConfiguration()
            //     .ReadFrom.Configuration(configuration)
            //     .WriteTo.MSSqlServer(configuration["ConnectionStrings:CompareHareDbContext"], "Logs",
            //         autoCreateSqlTable: true, restrictedToMinimumLevel: LogEventLevel.Warning)
            //     .CreateLogger();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.MySQL(configuration["ConnectionStrings:CompareHareDbContext"])
                .CreateLogger();
        }
    }
}
