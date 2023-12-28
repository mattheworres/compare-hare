using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
// using Serilog;
using Microsoft.AspNetCore;

namespace CompareHare.Api;
public class Program
{
    public static void Main(string[] args)
    {
        // ASP.NET Core 3.0+:
        // The UseServiceProviderFactory call attaches the
        // Autofac provider to the generic hosting mechanism.
        var host = Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();
            })
            .Build();

        host.Run();
    }
}
// public static class Program
// {
//     public static int Main(string[] args)
//     {
//         // SerilogConfigurator.Configure();

//         try
//         {
//             // Log.Information("Starting web host");

//             // CreateWebHostBuilder(args).Build().Run();
//             ConfigureWebApp(args);

//             return 0;
//         }
//         catch (Exception ex)
//         {
//             // Log.Fatal(ex, "Host terminated unexpectedly");
//             return 1;
//         }
//         finally
//         {
//             // Log.CloseAndFlush();
//         }
//     }

//     public static void ConfigureWebApp(string[] args) 
//     {
//         var builder = WebApplication.CreateBuilder(args);

//         // Add services to the container.

//         builder.Services.AddControllers();
//         // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//         builder.Services.AddEndpointsApiExplorer();
//         builder.Services.AddSwaggerGen();
//         // builder.Services.AddAutofac();
//         ConfigureAutofac();

//         var app = builder.Build();

//         // Configure the HTTP request pipeline.
//         if (app.Environment.IsDevelopment())
//         {
//             app.UseSwagger();
//             app.UseSwaggerUI();
//         }

//         app.UseHttpsRedirection();

//         app.UseAuthorization();

//         app.MapControllers();

//         // TODO: UseAutofacMiddleware?

//         app.Run();
//     }

//     private static void ConfigureAutofac()
//     {
//         var builder = new ContainerBuilder();

//         // Get your HttpConfiguration.
//         var config = GlobalConfiguration.Configuration;

//         // Register your Web API controllers.
//         builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

//         // OPTIONAL: Register the Autofac filter provider.
//         builder.RegisterWebApiFilterProvider(config);

//         // OPTIONAL: Register the Autofac model binder provider.
//         builder.RegisterWebApiModelBinderProvider();

//         // Set the dependency resolver to be Autofac.
//         var container = builder.Build();
//         config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
//     }
