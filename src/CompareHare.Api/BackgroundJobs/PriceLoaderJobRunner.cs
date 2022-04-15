using Autofac;
using CompareHare.Api.BackgroundJobs.Interfaces;

namespace CompareHare.Api.BackgroundJobs.JobRunners
{
    public class PriceLoaderJobRunner : SyncJobRunner<IPriceLoaderJob>
    {
        public PriceLoaderJobRunner(ILifetimeScope container)
            : base(container) { }
    }
}
