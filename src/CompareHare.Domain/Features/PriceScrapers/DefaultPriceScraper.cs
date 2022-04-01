using System;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Io;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Features.PriceScrapers.Interfaces;
using CompareHare.Domain.Services.Interfaces;
using Serilog;

namespace CompareHare.Domain.Features.PriceScrapers
{
    public class DefaultPriceScraper : IPriceScraper
    {
        private readonly string LOCAL_URL_BASE = "http://localhost:8000/public";
        private readonly IParserWrapper _parserWrapper;
        private readonly IParserHelper _parserHelper;
        private readonly IProductHelper _productHelper;

        public DefaultPriceScraper(IParserWrapper parserWrapper, IParserHelper parserHelper, IProductHelper productHelper)
        {
            _parserWrapper = parserWrapper;
            _parserHelper = parserHelper;
            _productHelper = productHelper;
        }

        public async Task<ProductRetailerPrice> ScrapePrice(int trackedProductId, ProductRetailer productRetailer, string productUrl, string priceSelector, IRequester requester = null)
        {
            // TODO: tie to environment somehow...
            // var document = await _parserWrapper.OpenUrlAsync(productUrl, requester);
            var localhostUrl = GetLocalhostProductUrl(productRetailer);
            Log.Logger.Information("Scraping price, localhost URL of {0} for retailer {1}", localhostUrl, productRetailer.ToString());
            var document = await _parserWrapper.OpenUrlAsync(localhostUrl, requester);
            var selector = _productHelper.GetRetailerSelector(productRetailer);
            var priceElement = document.QuerySelector(selector == null ? priceSelector : selector);

            return new ProductRetailerPrice
            {
                Id = 0,
                TrackedProductId = trackedProductId,
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
