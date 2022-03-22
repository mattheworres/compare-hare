using System.Globalization;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Io;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Features.PriceScrapers.Interfaces;
using CompareHare.Domain.Services.Interfaces;

namespace CompareHare.Domain.Features.PriceScrapers
{
    public class DefaultPriceScraper : IPriceScraper
    {
        private readonly IParserWrapper _parserWrapper;
        private readonly IParserHelper _parserHelper;
        private readonly IProductHelper _productHelper;

        public DefaultPriceScraper(IParserWrapper parserWrapper, IParserHelper parserHelper, IProductHelper productHelper)
        {
            _parserWrapper = parserWrapper;
            _parserHelper = parserHelper;
            _productHelper = productHelper;
        }

        public async Task<ProductRetailerPriceHistory> ScrapePrice(int trackedProductId, ProductRetailer productRetailer, string productUrl, string priceSelector, IRequester requester = null)
        {
            var document = await _parserWrapper.OpenUrlAsync(productUrl, requester);
            var selector = _productHelper.GetRetailerSelector(productRetailer);
            var priceElement = document.QuerySelector(selector == null ? priceSelector : selector);

            return new ProductRetailerPriceHistory
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

            // Maybe try the currency parser instead here for all... see if tests pass

            switch (productRetailer)
            {
                // No commas, no decimals. straight int
                case ProductRetailer.Lowes:
                case ProductRetailer.HomeDepot:
                    return float.Parse(roughScrapedPrice, CultureInfo.InvariantCulture.NumberFormat);

                // Can have commas, is a legitimate float
                case ProductRetailer.BestBuy:
                case ProductRetailer.AppliancesConnection:
                    var cleanPrice = _parserHelper.RemoveCommasFromString(roughScrapedPrice);
                    return _parserHelper.ParseFirstFloatFromString(cleanPrice);

                default:
                    return 0f;
            }
        }
    }
}