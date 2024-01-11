using CompareHare.Api.Features.Prices.Services.Interfaces;
using CompareHare.Domain.Entities;
using Serilog;
using CompareHare.Domain.Services.Interfaces;
using CompareHare.Domain.Entities.Constants;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Prices.Services
{
    public class PriceScraperRunner : IPriceScraperRunner
    {
        private const int THROTTLE_MIN = 3;
        private const int THROTTLE_MAX = 9;

        private readonly CompareHareDbContext _dbContext;
        private readonly IPriceScraperPicker _scraperPicker;
        private readonly IPricePersister _pricePersister;
        private readonly IProductHelper _productHelper;

        public PriceScraperRunner(CompareHareDbContext dbContext, IPriceScraperPicker scraperPicker, IPricePersister pricePersister, IProductHelper productHelper)
        {
            _productHelper = productHelper;
            _dbContext = dbContext;
            _scraperPicker = scraperPicker;
            _pricePersister = pricePersister;
        }

        public async Task LoadAllPrices(CancellationToken ct)
        {
            var activeProducts = _dbContext.TrackedProducts.Where(x => x.Enabled).ToList();

            Log.Logger.Information("Got {0} active products", activeProducts.Count());

            foreach (var product in activeProducts)
            {
                if (ct.IsCancellationRequested) {
                    return;
                }

                var random = new Random();

                var enabledRetailers = _dbContext.TrackedProductRetailers.Where(x => x.TrackedProductId == product.Id && x.Enabled == true).ToList();
                Log.Logger.Information("For product #{0} we have {1} retailers...", product.Id, enabledRetailers.Count());
                foreach (var productRetailer in enabledRetailers)
                {
                    if (ct.IsCancellationRequested) {
                        return;
                    }

                    // TODO: Verify this actually works with Thread.Sleep... but... ok?
                    var throttleSeconds = 2;//random.Next(THROTTLE_MIN, THROTTLE_MAX);
                    // var limiter = TimeLimiter.GetFromMaxCountByInterval(1, TimeSpan.FromSeconds(throttleSeconds));
                    Log.Logger.Information("About to scrape for retailer {0} for product {1}...", productRetailer.ProductRetailer.ToString(), product.Id);
                    var scraper = _scraperPicker.PickPriceScraper(productRetailer.ProductRetailer);

                    Log.Logger.Information("Throttling for {0} seconds, please wait...", throttleSeconds);

                    // await limiter;
                    Thread.Sleep(throttleSeconds * 1000);

                    Log.Logger.Information("Throttling complete, attempting to scrape...");

                    ProductRetailerPrice scrapedPrice;

                    //Prior to attempting scrape, remove existing ones to limit duplication
                    var existingExceptions = _dbContext.ProductPriceScrapingExceptions
                        .Where(x => x.TrackedProductId == product.Id && x.TrackedProductRetailerId == productRetailer.Id)
                        .ToList();
                    if (existingExceptions.Any())
                    {
                        _dbContext.RemoveRange(existingExceptions);
                    }

                    try
                    {
                        scrapedPrice = await scraper.ScrapePrice(product.Id, productRetailer.Id, productRetailer.ProductRetailer, productRetailer.ScrapeUrl, productRetailer.PriceSelector);
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Information("Got an exception, no can do for this one pal");

                        var retailer = productRetailer.ProductRetailer;
                        var retailerIsOther = retailer == ProductRetailer.Other;
                        var exceptionMessage = string.Format("{0} Stack Trace: {1}", ex.Message.ToString(), ex.StackTrace.ToString());
                        var newException = new ProductPriceScrapingException()
                        {
                            TrackedProductId = product.Id,
                            TrackedProductRetailerId = productRetailer.Id,
                            ProductRetailer = retailer,
                            Url = productRetailer.ScrapeUrl,
                            Selector = _productHelper.GetRetailerSelector(retailer, productRetailer.PriceSelector),
                            Error = exceptionMessage
                        };

                        await _dbContext.ProductPriceScrapingExceptions.AddAsync(newException);
                        _dbContext.SaveChanges();
                        continue;
                    }


                    var lastPrice = await _dbContext.ProductRetailerPrices
                        .Where(x => x.TrackedProductId == product.Id && x.TrackedProductRetailerId == productRetailer.Id)
                        .OrderByDescending(x => x.PriceDate)
                        .FirstOrDefaultAsync();
                    var lastStringee = lastPrice == null || !lastPrice.Price.HasValue ? "Nullee" : string.Format("${0}", lastPrice.Price.Value);
                    var lastPriceId = lastPrice != null ? (int?)lastPrice.Id : null;
                    // We want to persist this price as long as A) we have a price and either B) we haven't saved a price before or C) the price has changed in any way (in the future this gets more complicated)
                    if (scrapedPrice.Price.HasValue && (lastPrice == null || lastPrice.Price.Value != scrapedPrice.Price.Value))
                    {
                        Log.Logger.Information("Huzzah, we have a new price for {0} - ${1} (was {2})", productRetailer.ProductRetailer.ToString(), scrapedPrice.Price.Value, lastStringee);

                        _pricePersister.PersistNewPrice(scrapedPrice, lastPriceId);
                    }
                    else
                    {
                        var hasValue = scrapedPrice.Price.HasValue ? string.Format("Scraped price had value of ${0}", scrapedPrice.Price.Value) : "No scraped price value";
                        var hasLastPrice = lastPrice == null ? "Didn't have last price value" : string.Format("Had last price value: ${0}", lastPrice.Price.Value);

                        Log.Logger.Information("Bummer, no new price. hasValue: {0} hasLastPrice: {1}", hasValue, hasLastPrice);
                        // Not sure if we actually want to do this...date correlates to change
                        // if (lastPrice != null && scrapedPrice.Price.HasValue && scrapedPrice.Price.Value == lastPrice.Price.Value) {
                        //     _pricePersister.UpdateUnchangedPrice(lastPrice.Id, DateTime.Now.Date);
                        // }
                    }
                }
            }

            var countTotal = activeProducts.Sum(p => p.Retailers.Where(x => x.Enabled).Count());
            var activeProductRetailerIds = activeProducts.SelectMany(x => x.Retailers.Where(y => y.Enabled).Select(z => z.Id)).ToList();
            var exceptions = _dbContext.ProductPriceScrapingExceptions.Count(x => activeProductRetailerIds.Contains(x.TrackedProductRetailerId));
            var percentage = countTotal > 0 ? exceptions/countTotal * 100 : 0;

            Log.Logger.Information("Price scrape done, nice jerb. {0} total products, {1} exceptions ( {2}% fail)", countTotal, exceptions, percentage);
        }
    }
}
