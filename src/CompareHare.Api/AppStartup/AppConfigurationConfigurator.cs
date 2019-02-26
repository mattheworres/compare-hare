#region usings

using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

#endregion

namespace CompareHare.Api.AppStartup
{
    public static class AppConfigurationConfigurator
    {
        public static void Configure(WebHostBuilderContext hostingContext, IConfigurationBuilder config, string[] commandLineArgs)
        {
            var env = hostingContext.HostingEnvironment;
            config.AddJsonFile("appsettings.json", true, true)
                                .AddJsonFile(string.Format("appsettings.{0}.json", env.EnvironmentName), true, true)
                                .AddJsonFile(string.Format("appsettings.User.json", env.EnvironmentName), true, true);

            if (env.IsDevelopment())
            {
                var assembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                if (assembly != null)
                    config.AddUserSecrets(assembly, true);
            }

            config.AddEnvironmentVariables();

            if (commandLineArgs == null) return;

            config.AddCommandLine(commandLineArgs);
        }
    }
}
