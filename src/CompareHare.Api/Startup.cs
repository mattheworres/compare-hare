using System.Globalization;
using System.Reflection;
using Autofac;
using CompareHare.Api.AppStartup;
using CompareHare.Api.Controllers;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Hangfire;
using Autofac.Extensions.DependencyInjection;
using CompareHare.Api.BackgroundJobs.Configuration;

namespace CompareHare.Api;

public class Startup
{
    private readonly string MyCorsPolicy = "myCorsPolicy";
    public Startup (IConfiguration configuration, IWebHostEnvironment environment)
    {
        // In ASP.NET Core 3.x, using `Host.CreateDefaultBuilder` (as in the preceding Program.cs snippet) will
        // set up some configuration for you based on your appsettings.json and environment variables. See "Remarks" at
        // https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.host.createdefaultbuilder for details.
        _configuration = configuration;
        _environment = environment;
    }

    public IConfiguration _configuration;
    public IWebHostEnvironment _environment;

    public void ConfigureServices(IServiceCollection services)
    {
        var builder = new ContainerBuilder();

        SerilogConfigurator.Configure();

        services.Configure<RouteOptions>(options => {
            options.LowercaseUrls = true;
        });

        services.AddCors(options => {
            options.AddPolicy(name: MyCorsPolicy,
                builder => CorsPolicyConfigurator.Configure(builder));
        });

        services.AddOptions();
        services.AddFluentValidationAutoValidation();

        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = SharedAuthConstants.IdentityApplicationScheme;
        })
            // .AddCookie("Identity.Application");
            .AddCookie(SharedAuthConstants.IdentityApplicationScheme, options => {
                options.LoginPath = "/api/authentication/sign-in";
                options.AccessDeniedPath = "/api/error";
                options.ExpireTimeSpan = TimeSpan.FromHours(4);
            });

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
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

        // var appContainer = app.ApplicationServices.GetAutofacRoot();

        services.AddHangfire(config => HangfireConfig.ConfigureService(config, _configuration));
        // services.AddHangfire(config => HangfireConfig.Configure(config, AutofacContainer, _configuration, _hostingEnvironment));
        services.AddHangfireServer();

        services.AddSwaggerGen();

        // EF Identity:
        services.AddAuthorization();

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

    public void Configure(IApplicationBuilder app, CompareHareDbContext dbContext, IGlobalConfiguration globalConfiguration, IWebHostEnvironment env)
    {
        // If, for some reason, you need a reference to the built container, you
        // can use the convenience extension method GetAutofacRoot.
        var autofacContainer = app.ApplicationServices.GetAutofacRoot();
        HangfireConfig.ConfigureAndSchedule(globalConfiguration, autofacContainer, _configuration);

        app.UseHangfireDashboard();
        app.UseRouting();

        app.UseCors(MyCorsPolicy);
        app.UseAuthorization();
        var hangfireDashboardOptions = new DashboardOptions()
        {
            Authorization = new[] { new BackgroundJobsAuthorizationFilter(!env.IsProduction())},
            DashboardTitle = "CompareHare Jobs Dash",
        };
        app.UseEndpoints(endpoints => {
            endpoints.MapHealthChecks("/healthcheck")
                .RequireAuthorization();
            endpoints.MapControllerRoute(name: "api", pattern: "api/{controller}/{action}",
                defaults: new { action = "Get" })
                .RequireCors(MyCorsPolicy)
                ;
            endpoints.MapHangfireDashboard("/background-jobs", hangfireDashboardOptions);
        });

        // TODO: only for development
        app.UseSwagger();
        app.UseSwaggerUI();

        // TODO: check if migrations applied?
    }
}
