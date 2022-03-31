using System;
using System.Linq;
using System.Threading.Tasks;
using CompareHare.Api.Features.Prices.Services.Interfaces;
using CompareHare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using RateLimiter;
using Serilog;
using ComposableAsync;

namespace CompareHare.Api.Features.Prices.Services
{
    public class PriceScraperRunner : IPriceScraperRunner
    {
        private const int THROTTLE_SECONDS = 5;

        private readonly CompareHareDbContext _dbContext;
        private readonly IPriceScraperPicker _scraperPicker;
        private readonly IPriceHistoryPersister _priceHistoryPersister;

        public PriceScraperRunner(CompareHareDbContext dbContext, IPriceScraperPicker scraperPicker, IPriceHistoryPersister priceHistoryPersister)
        {
            _dbContext = dbContext;
            _scraperPicker = scraperPicker;
            _priceHistoryPersister = priceHistoryPersister;
        }

        public async Task LoadAllPrices()
        {
            var activeProducts = await _dbContext.TrackedProducts.Where(x => x.Enabled).ToListAsync();

            Log.Logger.Information("Got {0} active products", activeProducts.Count());

            var limiter = TimeLimiter.GetFromMaxCountByInterval(1, TimeSpan.FromSeconds(THROTTLE_SECONDS));

            foreach (var product in activeProducts)
            {
                var enabledRetailers = await _dbContext.TrackedProductRetailers.Where(x => x.TrackedProductId == product.Id && x.Enabled == true).ToListAsync();
                Log.Logger.Information("For product #{0} we have {1} retailers...", product.Id, enabledRetailers.Count());
                foreach (var productRetailer in enabledRetailers)
                {
                    Log.Logger.Information("About to scrape for retailer {0} for product {1}...", productRetailer.ProductRetailer.ToString(), product.Id);
                    var scraper = _scraperPicker.PickPriceScraper(productRetailer.ProductRetailer);

                    await limiter;

                    var scrapedPriceHistory = await scraper.ScrapePrice(product.Id, productRetailer.ProductRetailer, productRetailer.ScrapeUrl, productRetailer.PriceSelector);
                    var lastPriceHistory = await _dbContext.ProductRetailerPriceHistories
                        .Where(x => x.TrackedProductId == product.Id && x.ProductRetailer == productRetailer.ProductRetailer)
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefaultAsync();
                    var lastStringee = lastPriceHistory == null || !lastPriceHistory.Price.HasValue ? "Nullee" : string.Format("${0}", lastPriceHistory.Price.Value);

                    // We want to persist this price as long as A) we have a price and either B) we haven't saved a price before or C) the price has changed in any way (in the future this gets more complicated)
                    if (scrapedPriceHistory.Price.HasValue && (lastPriceHistory == null || lastPriceHistory.Price.Value != scrapedPriceHistory.Price.Value))
                    {
                        Log.Logger.Information("Huzzah, we have a new price for {0} - ${1} (was {2})", productRetailer.ProductRetailer.ToString(), scrapedPriceHistory.Price.Value, lastStringee);

                        await _priceHistoryPersister.PersistNewPriceHistory(scrapedPriceHistory);
                    }
                    else
                    {
                        var hasValue = scrapedPriceHistory.Price.HasValue ? string.Format("Scraped price had value of ${0}", scrapedPriceHistory.Price.Value) : "No scraped price value";
                        var hasLastPriceHistory = lastPriceHistory == null ? "Didn't have last price value" : string.Format("Had last price value: ${0}", lastPriceHistory.Price.Value);

                        Log.Logger.Information("Bummer, no new price. hasValue: {0} hasLastPriceHistory: {1}", hasValue, hasLastPriceHistory);
                    }
                }
            }

            Log.Logger.Information("Price scrape done, nice jerb.");
        }
    }
}