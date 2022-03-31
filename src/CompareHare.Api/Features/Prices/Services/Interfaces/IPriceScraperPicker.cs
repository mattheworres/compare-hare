using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Features.Interfaces;
using CompareHare.Domain.Features.PriceScrapers.Interfaces;

namespace CompareHare.Api.Features.Prices.Services.Interfaces
{
    public interface IPriceScraperPicker : IFeatureService
    {
        IPriceScraper PickPriceScraper(ProductRetailer retailer);
    }
}