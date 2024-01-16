using System.Reflection;
using Autofac;
using CompareHare.Api.BackgroundJobs.Attributes;
using CompareHare.Api.BackgroundJobs.Interfaces;
using Hangfire;
using Serilog;

namespace CompareHare.Api.BackgroundJobs
{
    public class JobRunner<TJob> : IJobRunner<TJob>
        where TJob : IJob
    {
        private ILifetimeScope _container;

        public JobRunner(ILifetimeScope container)
        {
            _container = container;
        }

        public async Task Run(IJobCancellationToken jobCancellationToken)
        {
            using (var lifetimeScope = _container.BeginLifetimeScope())
            {
                var jobs = lifetimeScope.Resolve<IEnumerable<TJob>>();
                foreach (var job in jobs.OrderBy(j => j.GetType().GetCustomAttribute<JobOrderAttribute>()?.Order ?? int.MaxValue))
                {
                    try
                    {
                        await job.Run(jobCancellationToken.ShutdownToken);
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(ex, "JobRunner: Error running job {0}", job.GetType().Name);
                    }
                }
            }
        }
    }
}
