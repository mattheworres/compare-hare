using System.Threading;
using System.Threading.Tasks;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.BackgroundJobs.Interfaces
{
    public interface IJob : IFeatureService
    {
        void Run(CancellationToken ct);
    }
}
