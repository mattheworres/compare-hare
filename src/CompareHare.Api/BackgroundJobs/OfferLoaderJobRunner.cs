using Autofac;
using CompareHare.Api.BackgroundJobs;
using CompareHare.Api.BackgroundJobs.Interfaces;

namespace Bookendz.Api.BackgroundJobs.JobRunners
{
    public class OfferLoaderJobRunner : JobRunner<IOfferLoaderJob>
    {
        public OfferLoaderJobRunner(ILifetimeScope container)
            : base(container) { }
    }
}
