using System.Globalization;
using System.Reflection;
using Autofac;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api;

public class Startup
{
    public Startup (IConfiguration configuration)
    {
        // In ASP.NET Core 3.x, using `Host.CreateDefaultBuilder` (as in the preceding Program.cs snippet) will
        // set up some configuration for you based on your appsettings.json and environment variables. See "Remarks" at
        // https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.host.createdefaultbuilder for details.
        this._configuration = configuration;
    }

    public IConfiguration _configuration;
    public ILifetimeScope AutofacContainer;

    // ConfigureServices is where you register dependencies. This gets
    // called by the runtime before the ConfigureContainer method, below.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the collection. Don't build or return
        // any IServiceProvider or the ConfigureContainer method
        // won't get called. Don't create a ContainerBuilder
        // for Autofac here, and don't call builder.Populate() - that
        // happens in the AutofacServiceProviderFactory for you.
        var builder = new ContainerBuilder();
        services.AddOptions();
        // services.AddDbContext<CompareHareDbContext>(
        //     options => EntityFrameworkConfigurator.Configure(_configuration, options), ServiceLifetime.Scoped);
        services.AddDbContext<CompareHareDbContext>(options => options.UseInMemoryDatabase("CompareHareInMem"));
        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1.0);
            options.Lockout.MaxFailedAccessAttempts = 100;
        })
            .AddEntityFrameworkStores<CompareHareDbContext>()
            .AddDefaultTokenProviders();

        services.AddMvc()
                // commented for now, as this seems broken signature-wise?
                // .AddJsonOptions(JsonOptionsConfigurator.Configure)
                .AddDataAnnotationsLocalization();
        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");

                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;
            });
        // EF Identity:
        services.AddAuthorization();
        //TODO: add HangFire
        
        services.AddHealthChecks();
    }

    // ConfigureContainer is where you can register things directly
    // with Autofac. This runs after ConfigureServices so the things
    // here will override registrations made in ConfigureServices.
    // Don't build the container; that gets done for you by the factory.
    public void ConfigureContainer(ContainerBuilder builder)
    {
        // Register your own things directly with Autofac here. Don't
        // call builder.Populate(), that happens in AutofacServiceProviderFactory
        // for you.
        // builder.RegisterModule(new MyApplicationModule());
        builder.RegisterAssemblyModules(
                typeof(IFeatureService).Assembly,
                Assembly.GetExecutingAssembly());
    }

    // Configure is where you add middleware. This is called after
    // ConfigureContainer. You can use IApplicationBuilder.ApplicationServices
    // here if you need to resolve things from the container.
    public void Configure(
      IApplicationBuilder app,
      ILoggerFactory loggerFactory)
    {
        // If, for some reason, you need a reference to the built container, you
        // can use the convenience extension method GetAutofacRoot.
        // this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

        // loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
        // loggerFactory.AddDebug();
        // app.UseMvc();
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}