#region usings

using System;
using System.IO;
using Autofac.Extensions.DependencyInjection;
using CompareHare.Api.AppStartup;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Microsoft.AspNetCore;

#endregion

namespace CompareHare.Api
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            SerilogConfigurator.Configure();

            try
            {
                Log.Information("Starting web host");

                CreateWebHostBuilder(args).Build().Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .ConfigureServices(services => services.AddAutofac())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration(
                    (hostingContext, configurationBuilder) =>
                        AppConfigurationConfigurator.Configure(hostingContext, configurationBuilder, args))
                .UseIISIntegration()
                .UseDefaultServiceProvider((context, options) => options.ValidateScopes = context.HostingEnvironment.IsDevelopment())
                .UseStartup<Startup>()
                .UseSerilog()
                ;
    }
}
