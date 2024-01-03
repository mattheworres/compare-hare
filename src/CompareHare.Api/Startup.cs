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
        // TODO: get CORS enabled
        // services.AddCors(options => {
        //     options.AddPolicy(name: MyCorsPolicy,
        //         policy => {
        //             policy.WithOrigins("http://localhost:8000", "https://localhost:8000");
        //         });
        // });
        services.AddOptions();
        services.AddFluentValidationAutoValidation();

        // services.AddDbContext<CompareHareDbContext>(options => options.UseInMemoryDatabase("CompareHareInMem"));
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<CompareHareDbContext>()
            .AddApiEndpoints()
            .AddRoles<Role>();
        // services.AddIdentity<User, Role>(options =>
        // {
        //     options.Password.RequireDigit = false;
        //     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1.0);
        //     options.Lockout.MaxFailedAccessAttempts = 100;
        // })
        //     .AddEntityFrameworkStores<CompareHareDbContext>()
        //     .AddDefaultTokenProviders();

        services.AddDbContext<CompareHareDbContext>(
            options => EntityFrameworkConfigurator.Configure(_configuration, options), ServiceLifetime.Scoped);

        // services.AddCors(options => CorsPolicyConfigurator.Configure(options));
        services.AddMvc()
                // commented for now, as this seems broken signature-wise?
                // .AddJsonOptions(JsonOptionsConfigurator.Configure)
                .AddDataAnnotationsLocalization();

        // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

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

        // CheckConnection(dbContext);

        // TODO: check if migrations applied?
    }

    private static bool CheckConnection(CompareHareDbContext dbContext) {
        try {
            if (dbContext.Database.CanConnect()) {
                dbContext.Database.OpenConnection();
                dbContext.Database.CloseConnection();
            } else {
                throw new Exception("Database can't connect");
            }
        } catch(Exception ex) {
            Console.WriteLine($"DB Exception: {ex.Message}");
            return false;
        }

        Console.WriteLine("DB Connected! Yay!");
        return true;
    }
}
