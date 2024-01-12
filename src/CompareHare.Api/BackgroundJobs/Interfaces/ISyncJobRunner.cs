namespace CompareHare.Api.BackgroundJobs.Interfaces
{
    public interface ISyncJobRunner<TJob>
        where TJob : ISyncJob
    {
        void Run();
    }
}
