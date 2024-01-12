using System.Reflection;
using Autofac;
using CompareHare.Api.BackgroundJobs.Attributes;
using CompareHare.Api.BackgroundJobs.Interfaces;
using Serilog;

namespace CompareHare.Api.BackgroundJobs
{
    public class SyncJobRunner<TJob> : ISyncJobRunner<TJob>
        where TJob : ISyncJob
    {
        private ILifetimeScope _container;

        public SyncJobRunner(ILifetimeScope container)
        {
            _container = container;
        }

        public void Run()
        {
            using (var lifetimeScope = _container.BeginLifetimeScope())
            {
                var jobs = lifetimeScope.Resolve<IEnumerable<TJob>>();
                foreach (var job in jobs.OrderBy(j => j.GetType().GetCustomAttribute<JobOrderAttribute>()?.Order ?? int.MaxValue))
                {
                    try
                    {
                        job.Run();
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(ex, "Error running job {0}", job.GetType().Name);
                    }
                }
            }
        }
    }
}
