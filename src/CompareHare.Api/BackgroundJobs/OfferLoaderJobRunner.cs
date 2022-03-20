using Autofac;
using CompareHare.Api.BackgroundJobs.Interfaces;

namespace CompareHare.Api.BackgroundJobs.JobRunners
{
    public class OfferLoaderJobRunner : JobRunner<IOfferLoaderJob>
    {
        public OfferLoaderJobRunner(ILifetimeScope container)
            : base(container) { }
    }
}
