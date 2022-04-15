using AngleSharp.Io;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Domain.Features.PriceScrapers.Interfaces
{
    public interface IPriceScraper : IFeatureService
    {
        // For now, default price scraper needs retailer URL and the price selector, but we may need different string data for specific scrapers. Per retailer basis
        ProductRetailerPrice ScrapePrice(int trackedProductId, int trackedProductRetailerId, ProductRetailer productRetailer, string productIdentifier1, string productIdentifier2, IRequester requester = null);
    }
}
