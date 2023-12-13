using System;
using CompareHare.Api.Features.Prices.Services.Interfaces;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Features.PriceScrapers;
using CompareHare.Domain.Features.PriceScrapers.Interfaces;

namespace CompareHare.Api.Features.Prices.Services
{
    public class PriceScraperPicker : IPriceScraperPicker
    {
        private readonly Lazy<DefaultPriceScraper> _defaultPriceScraper;

        public PriceScraperPicker(Lazy<DefaultPriceScraper> defaultPriceScraper)
        {
            _defaultPriceScraper = defaultPriceScraper;
        }

        public IPriceScraper PickPriceScraper(ProductRetailer retailer)
        {
            switch (retailer)
            {
                default:
                    return _defaultPriceScraper.Value;
            }
        }
    }
}