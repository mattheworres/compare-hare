using Autofac;
using CompareHare.Api.BackgroundJobs.Interfaces;

namespace CompareHare.Api.BackgroundJobs
{
    public class DefaultRecurringJobRunner : JobRunner<IDefaultRecurringJob>
    {
        public DefaultRecurringJobRunner(ILifetimeScope container) : base(container) { }
    }
}
