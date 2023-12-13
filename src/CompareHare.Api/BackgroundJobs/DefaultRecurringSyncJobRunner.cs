using Autofac;
using CompareHare.Api.BackgroundJobs.Interfaces;

namespace CompareHare.Api.BackgroundJobs
{
    public class DefaultRecurringSyncJobRunner : SyncJobRunner<IDefaultRecurringSyncJob>
    {
        public DefaultRecurringSyncJobRunner(ILifetimeScope container) : base(container) { }
    }
}
