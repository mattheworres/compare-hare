using Hangfire;

namespace CompareHare.Api.BackgroundJobs.Interfaces
{
    public interface IJobRunner<TJob>
        where TJob : IJob
    {
        Task Run(IJobCancellationToken cancellationToken);
    }
}
