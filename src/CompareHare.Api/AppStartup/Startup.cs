#region usings

using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.Variance;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Interfaces;
using CompareHare.Api.Extensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using CompareHare.Api.Filters;
using System;
using Hangfire;
using CompareHare.Api.BackgroundJobs.Configuration;

#endregion

namespace CompareHare.Api.AppStartup
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private IContainer _applicationContainer;

        private ILoggerFactory _loggerFactory;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _loggerFactory = loggerFactory;
        }

        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            ConfigureContainer(builder);

            services.AddOptions();

            services.AddDbContext<CompareHareDbContext>(
                 options => EntityFrameworkConfigurator.Configure(_configuration, options), ServiceLifetime.Scoped);

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1.0);
                options.Lockout.MaxFailedAccessAttempts = 100;
            })
                .AddEntityFrameworkStores<CompareHareDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => CookieConfigurator.Configure(options, _hostingEnvironment));

            services.AddMvc()
                .AddJsonOptions(JsonOptionsConfigurator.Configure)
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

            services.AddHangfire(config => HangfireConfig.Configure(config, _applicationContainer, _configuration, _hostingEnvironment));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<NoCacheFilter>();

            // Make validators look at camelCase instead of PascalCase
            ValidatorOptions.PropertyNameResolver = ValidatorConfigurator.ResolvePropertyName;

            builder.Populate(services);

            _applicationContainer = builder.Build();
            return new AutofacServiceProvider(_applicationContainer);
        }

        [UsedImplicitly]
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());

            builder.RegisterAssemblyModules(
                typeof(IFeatureService).Assembly,
                Assembly.GetExecutingAssembly());

            builder.RegisterInstance(_configuration).As<IConfiguration>();
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, CompareHareDbContext dbContext)
        {
            var appIsDevOrQa = env.IsDevelopment() || env.IsEnvironment("QA");
            if (appIsDevOrQa)
            {
                app.UseDeveloperExceptionPage();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            })
               .UseStaticFiles()
               .UseCors(CorsPolicyConfigurator.Configure)
               .UseAuthentication()
               .UseHangfireServer(new BackgroundJobServerOptions()
               {
                   WorkerCount = 1,
               })
               .UseHangfireDashboard("/background-jobs", new DashboardOptions
               {
                   Authorization = new[] { new BackgroundJobsAuthorizationFilter(!env.IsProduction()) }
               })
               .UseMvc(RouteConfigurator.Configure);

            if (!dbContext.AllMigrationsApplied())
            {
                dbContext.Database.Migrate();
            }

            dbContext.EnsureSeeded(serviceProvider.GetService<UserManager<User>>(), serviceProvider.GetService<RoleManager<Role>>());
        }
    }
}
