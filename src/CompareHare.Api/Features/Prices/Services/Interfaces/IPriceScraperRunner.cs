using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.Features.Prices.Services.Interfaces
{
    public interface IPriceScraperRunner : IFeatureService
    {
        Task LoadAllPrices(CancellationToken ct);
    }
}
