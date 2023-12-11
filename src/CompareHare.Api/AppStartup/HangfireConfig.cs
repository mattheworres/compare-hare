#region usings

using System;
using System.Threading.Tasks;
using Autofac;
using CompareHare.Api.BackgroundJobs.Interfaces;
using Hangfire;
using Hangfire.MySql.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using MySql.Data.MySqlClient;


#endregion

namespace CompareHare.Api.AppStartup
{
    public static class HangfireConfig
    {
        public static void Configure(IGlobalConfiguration globalConfiguration, IContainer container, IConfiguration configuration, IHostingEnvironment env)
        {
            var connectionString = configuration["Hangfire:ConnectionString"];
            globalConfiguration.UseStorage(new MySqlStorage(BuildConnectionString(connectionString)));
            globalConfiguration.UseAutofacActivator(container);

            //ScheduleRecurringJobs<IDefaultRecurringJob>("BatchJobs", configuration, container, configuration["Hangfire:BatchJobSchedule"]);
            ScheduleRecurringJobs<IOfferLoaderJob>("OfferLoaderJob", configuration, container, configuration["Hangfire:OfferLoaderRunnerSchedule"]);
            ScheduleRecurringJobs<IAlertAssessorJob>("AlertAssessorJob", configuration, container, configuration["Hangfire:AlertAssessorRunnerSchedule"]);
            ScheduleRecurringJobs<IPriceLoaderJob>("PriceLoaderJob", configuration, container, configuration["Hangfire:PriceScraperRunnerSchedule"]);
        }

        private static string BuildConnectionString(string connectionString)
        {
            var builder = new MySqlConnectionStringBuilder(connectionString);
            builder.AllowUserVariables = true; // Required by Hangfire
            return builder.ToString();
        }

        private static void ScheduleRecurringJobs<TJob>(string jobId, IConfiguration configuration, IContainer container, string schedule)
            where TJob : IJob
        {
            if (!string.IsNullOrEmpty(schedule))
            {
                RecurringJob.AddOrUpdate<IJobRunner<TJob>>(jobId, jobRunner => jobRunner.Run(JobCancellationToken.Null), schedule, EasternTimeZone());
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
                RecurringJob.AddOrUpdate<ISyncJobRunner<TJob>>(jobId, jobRunner => jobRunner.Run(), schedule, EasternTimeZone());
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
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
            }
        }
    }
}
