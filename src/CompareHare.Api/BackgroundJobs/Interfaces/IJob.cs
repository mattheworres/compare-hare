using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.BackgroundJobs.Interfaces
{
    public interface IJob : IFeatureService
    {
        Task Run(CancellationToken ct);
    }
}
