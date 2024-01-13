using Autofac;
using CompareHare.Api.BackgroundJobs.Interfaces;
using Hangfire;
using Serilog;
using MySqlConnector;
using Hangfire.MySql;

namespace CompareHare.Api.AppStartup
{
    #pragma warning disable CS8604 // Possible null reference argument.
    public static class HangfireConfig
    {
        public static void ConfigureService(IGlobalConfiguration globalConfiguration, IConfiguration configuration) {
            var rawConnectionString = configuration["Hangfire:ConnectionString"] ?? "";
            var connectionString = BuildConnectionString(rawConnectionString);

            if (string.IsNullOrEmpty(connectionString)) {
                throw new Exception("No connecetion string for Hangfire");
            }

            globalConfiguration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions()))
                ;
        }

        public static void ConfigureAndSchedule(IGlobalConfiguration globalConfiguration, ILifetimeScope container, IConfiguration configuration)
        {
            globalConfiguration.UseAutofacActivator(container, false);

            var offerLoaderSchedule = configuration["Hangfire:OfferLoaderRunnerSchedule"] ?? "";
            var alertAssessorSchedule = configuration["Hangfire:AlertAssessorRunnerSchedule"] ?? "";
            var priceLoaderSchedule = configuration["Hangfire:PriceScraperRunnerSchedule"] ?? "";

            // Matt 2024: not sure why this was commented, may have been having issues?
            // ScheduleRecurringJobs<IDefaultRecurringJob>("BatchJobs", configuration, container, configuration["Hangfire:BatchJobSchedule"]);
            ScheduleRecurringJobs<IOfferLoaderJob>("OfferLoaderJob", configuration, container, offerLoaderSchedule);
            ScheduleRecurringJobs<IAlertAssessorJob>("AlertAssessorJob", configuration, container, alertAssessorSchedule);
            ScheduleRecurringJobs<IPriceLoaderJob>("PriceLoaderJob", configuration, container, priceLoaderSchedule);
        }

        private static string BuildConnectionString(string connectionString)
        {
            var builder = new MySqlConnectionStringBuilder(connectionString)
            {
                AllowUserVariables = true // Required by Hangfire
            };
            return builder.ToString();
        }

        private static void ScheduleRecurringJobs<TJob>(string jobId, IConfiguration configuration, ILifetimeScope container, string schedule)
            where TJob : IJob
        {
            if (!string.IsNullOrEmpty(schedule))
            {
                RecurringJob.AddOrUpdate<IJobRunner<TJob>>(jobId, jobRunner => jobRunner.Run(JobCancellationToken.Null), schedule);
            }
            else
            {
                RecurringJob.RemoveIfExists(jobId);
            }

            if (configuration["Hangfire:RunJobsOnStartup"] == "True")
            {
                Task.Run(async () =>
                {
                    try
                    {
                        using (var lifetimeScope = container.BeginLifetimeScope())
                        {
                            await lifetimeScope.Resolve<IJobRunner<TJob>>().Run(new JobCancellationToken(false));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error starting jobs");
                    }
                });
            }
        }

        private static void ScheduleRecurringSyncJobs<TJob>(string jobId, IConfiguration configuration, IContainer container, string schedule)
            where TJob : ISyncJob
        {
            if (!string.IsNullOrEmpty(schedule))
            {
                var options = new RecurringJobOptions
                {
                    TimeZone = EasternTimeZone()
                };
                RecurringJob.AddOrUpdate<ISyncJobRunner<TJob>>(jobId, jobRunner => jobRunner.Run(), schedule, options);
            }
            else
            {
                RecurringJob.RemoveIfExists(jobId);
            }

            if (configuration["Hangfire:RunJobsOnStartup"] == "True")
            {
                try
                {
                    using (var lifetimeScope = container.BeginLifetimeScope())
                    {
                        lifetimeScope.Resolve<ISyncJobRunner<TJob>>().Run();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error starting jobs");
                }
            }
        }

        private static TimeZoneInfo EasternTimeZone()
        {
            return TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
        }
    }
    #pragma warning restore CS8604 // Possible null reference argument.
}
