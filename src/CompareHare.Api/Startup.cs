using System.Globalization;
using System.Reflection;
using Autofac;
using CompareHare.Api.AppStartup;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace CompareHare.Api;

public class Startup
{
    private readonly string MyCorsPolicy = "myCorsPolicy";
    public Startup (IConfiguration configuration)
    {
        // In ASP.NET Core 3.x, using `Host.CreateDefaultBuilder` (as in the preceding Program.cs snippet) will
        // set up some configuration for you based on your appsettings.json and environment variables. See "Remarks" at
        // https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.host.createdefaultbuilder for details.
        this._configuration = configuration;
    }

    public IConfiguration _configuration;
    public ILifetimeScope AutofacContainer;

    public void ConfigureServices(IServiceCollection services)
    {
        var builder = new ContainerBuilder();
        // TODO: get CORS enabled
        // services.AddCors(options => {
        //     options.AddPolicy(name: MyCorsPolicy,
        //         policy => {
        //             policy.WithOrigins("http://localhost:8000", "https://localhost:8000");
        //         });
        // });
        services.AddOptions();
        services.AddFluentValidationAutoValidation();

        services.AddAuthentication().AddCookie("Identity.Application");

        // services.AddDbContext<CompareHareDbContext>(options => options.UseInMemoryDatabase("CompareHareInMem"));
        services.AddIdentityCore<User>()
            .AddRoles<Role>()
            .AddEntityFrameworkStores<CompareHareDbContext>()
            .AddApiEndpoints()
            ;

        services.AddDbContext<CompareHareDbContext>(
            options => EntityFrameworkConfigurator.Configure(_configuration, options), ServiceLifetime.Scoped);

        services.AddMvc()
                // commented for now, as this seems broken signature-wise?
                // .AddJsonOptions(JsonOptionsConfigurator.Configure)
                .AddDataAnnotationsLocalization();

        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(options =>
            {
                var enUS = "en-US";
                var supportedCultures = new[]
                {
                    new CultureInfo(enUS),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: enUS, uiCulture: enUS);

                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;
            });

        services.AddSwaggerGen();

        // EF Identity:
        services.AddAuthorization();
        //TODO: add HangFire

        services.AddHealthChecks();

    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        // Register your own things directly with Autofac here. Don't
        // call builder.Populate(), that happens in AutofacServiceProviderFactory
        // for you.
        builder.RegisterAssemblyModules(
                typeof(IFeatureService).Assembly,
                Assembly.GetExecutingAssembly());
    }

    public void Configure(IApplicationBuilder app, CompareHareDbContext dbContext)
    {
        // If, for some reason, you need a reference to the built container, you
        // can use the convenience extension method GetAutofacRoot.
        // this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
        app.UseRouting();

        // app.UseCors(builder => CorsPolicyConfigurator.Configure(builder));
        app.UseCors(MyCorsPolicy);
        app.UseAuthorization();
        app.UseEndpoints(endpoints => {
            endpoints.MapHealthChecks("/healthcheck")
                .RequireAuthorization();
            endpoints.MapControllerRoute(name: "api", pattern: "api/{controller}/{action}",
                defaults: new { action = "Get" })
                .RequireCors(MyCorsPolicy)
                ;

            // var api = endpoints.MapGroup("api");

            // api.MapControllerRoute(name: "api", pattern: "{controller}/{action}",
            //     defaults: new { action = "Get" });

            // api.MapControllers();
            // api.MapDefaultControllerRoute();
            // api.RequireCors(MyCorsPolicy);
            // TODO: Remove weather forecast once we know API is healthy and working
            // endpoints.MapControllerRoute(name: "WeatherForecast", pattern: "weatherforecast",
            //     defaults: new { controller = "WeatherForecast", action = "Get" })
            //     .RequireCors(MyCorsPolicy);// not sure this actually does anything

            // var apiGroup = endpoints.MapGroup("/api")
            //     .AddFluentValidationAutoValidation()
            //     .RequireCors();
            // apiGroup.MapControllers();
        });

        // TODO: only for development
        app.UseSwagger();
        app.UseSwaggerUI();

        // TODO: check if migrations applied?
    }
}
