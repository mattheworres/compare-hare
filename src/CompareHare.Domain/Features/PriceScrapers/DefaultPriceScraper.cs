using System;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Io;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Features.PriceScrapers.Interfaces;
using CompareHare.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace CompareHare.Domain.Features.PriceScrapers
{
    public class DefaultPriceScraper : IPriceScraper
    {
        private readonly string LOCAL_URL_BASE = "http://localhost:8000/public";
        private readonly IParserWrapper _parserWrapper;
        private readonly IParserHelper _parserHelper;
        private readonly IProductHelper _productHelper;
        // private readonly IHostingEnvironment _hostingEnvironment;

        public DefaultPriceScraper(IParserWrapper parserWrapper, IParserHelper parserHelper, IProductHelper productHelper/*, IHostingEnvironment hostingEnvironment*/)
        {
            // _hostingEnvironment = hostingEnvironment;
            _parserWrapper = parserWrapper;
            _parserHelper = parserHelper;
            _productHelper = productHelper;
        }

        public async Task<ProductRetailerPrice> ScrapePrice(int trackedProductId, int trackedProductRetailerId, ProductRetailer productRetailer, string productUrl, string priceSelector, IRequester requester = null)
        {
            // var urlToScrape = _hostingEnvironment.IsDevelopment() ? GetLocalhostProductUrl(productRetailer) : productUrl;
            // var urlToScrape = productUrl;
            var urlToScrape = GetLocalhostProductUrl(productRetailer);
            Log.Logger.Information("Scraping price, URL of {0} for retailer {1}", urlToScrape, productRetailer.ToString());
            var document = await _parserWrapper.OpenUrlAsync(urlToScrape, requester);
            Log.Logger.Information("URL opening complete, waiting now...");
            // await Task.Delay(5000);
            // Log.Logger.Information("Sleep is done, whoopie");
            var selector = _productHelper.GetRetailerSelector(productRetailer);
            var priceElement = document.QuerySelector(selector == null ? priceSelector : selector);

            if (productUrl.IndexOf("localhost") != -1 && priceElement == null) {
                throw new Exception(document.DocumentElement.OuterHtml);
            }
            // var message = priceElement != null ? "we have an element!!" : "no element to speak of, sad panda";
            // Log.Logger.Information("And done searching for element." + message);

            return new ProductRetailerPrice
            {
                Id = 0,
                TrackedProductId = trackedProductId,
                TrackedProductRetailerId = trackedProductRetailerId,
                ProductRetailer = productRetailer,
                Price = ParsePriceByRetailer(priceElement, productRetailer)
            };
        }

        private float ParsePriceByRetailer(IElement priceElement, ProductRetailer productRetailer)
        {
            var selector = _productHelper.GetRetailerSelector(productRetailer);
            var roughScrapedPrice = priceElement.Text().Trim();

            return _parserHelper.ParseCurrencyWithSymbol(roughScrapedPrice);
        }

        private string GetLocalhostProductUrl(ProductRetailer retailer)
        {

            switch (retailer)
            {
                case ProductRetailer.AppliancesConnection:
                    return string.Format("{0}/AppliancesConnection_Response.html", LOCAL_URL_BASE);

                case ProductRetailer.BestBuy:
                    return string.Format("{0}/BB_Response.html", LOCAL_URL_BASE);

                case ProductRetailer.HomeDepot:
                    return string.Format("{0}/HD_Response.html", LOCAL_URL_BASE);

                case ProductRetailer.Lowes:
                    return string.Format("{0}/Lowes_Response.html", LOCAL_URL_BASE);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
