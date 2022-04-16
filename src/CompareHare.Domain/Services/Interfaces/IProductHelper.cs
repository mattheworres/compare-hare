using AngleSharp;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Domain.Services.Interfaces
{
    public interface IProductHelper : IFeatureService
    {
        string GetRetailerSelector(ProductRetailer retailer, string defaultSelector);
        IConfiguration GetRetailerScrapeConfiguration(ProductRetailer retailer);
    }
}
