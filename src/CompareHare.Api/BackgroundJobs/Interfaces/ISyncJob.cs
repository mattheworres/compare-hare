using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.BackgroundJobs.Interfaces
{
    public interface ISyncJob : IFeatureService
    {
        void Run();
    }
}
