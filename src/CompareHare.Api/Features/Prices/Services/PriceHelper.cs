using CompareHare.Api.Features.Prices.Services.Interfaces;

namespace CompareHare.Api.Features.Prices.Services
{
    public class PriceHelper : IPriceHelper
    {
        public float CalculatePriceChange(float newPrice, float oldPrice)
        {
            return newPrice - oldPrice;
        }

        public float CalculatePriceChangePercentage(float newPrice, float oldPrice)
        {
            var change = CalculatePriceChange(newPrice, oldPrice);

            return oldPrice > 0 ? change / oldPrice : 1;
        }
    }
}