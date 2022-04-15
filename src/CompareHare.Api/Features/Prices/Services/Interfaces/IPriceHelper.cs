using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.Features.Prices.Services.Interfaces
{
    public interface IPriceHelper : IFeatureService
    {
        float CalculatePriceChange(float newPrice, float oldPrice);
        float CalculatePriceChangePercentage(float newPrice, float oldPrice);
    }
}
