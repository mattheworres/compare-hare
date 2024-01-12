using Autofac;
using CompareHare.Api.BackgroundJobs.Interfaces;

namespace CompareHare.Api.BackgroundJobs
{
    public class AlertAssessorJobRunner : JobRunner<IAlertAssessorJob>
    {
        public AlertAssessorJobRunner(ILifetimeScope container)
            : base(container) { }
    }
}
