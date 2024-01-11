using System.Net;
using AngleSharp;
using AngleSharp.Io;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Services.Interfaces;

namespace CompareHare.Domain.Services
{
    public class ProductHelper : IProductHelper
    {
        private readonly IRequesterHelper _requesterHelper;

        public ProductHelper(IRequesterHelper requesterHelper)
        {
            _requesterHelper = requesterHelper;
        }

        public IConfiguration GetRetailerScrapeConfiguration(ProductRetailer retailer)
        {
            var loaderOptions = _requesterHelper.GetDefaultLoaderOptions();
            var defaultRequester = _requesterHelper.GetDefaultRequester();
            string cookies, userAgent;
            WebHeaderCollection headers;
            IRequester requester;

            switch (retailer)
            {
                case ProductRetailer.HomeDepot:
                    var hdCookies = new Dictionary<string, string>() {
                        {"DELIVERY_ZIP", "16148"},
                        {"DELIVERY_ZIP_TYPE", "USER"},
                        {"IN_STORE_API_SESSION", "TRUE"},
                        {"WORKFLOW", "LOCALIZED_BY_GPS_MEDIUM"},
                        {"IN_STORE_USER_NUMBER", "Not%20In%20Store"},
                        {"HD_DC", "beta"}
                    };
                    cookies = _requesterHelper.GetCookieString(hdCookies);
                    userAgent = _requesterHelper.GetRandomUserAgentString();
                    headers = _requesterHelper.GetHeaders(userAgent, cookies);
                    requester = _requesterHelper.GetRequester(headers);

                    return Configuration.Default
                        .WithJs()
                        // .WithCulture("en-US")
                        .WithEventLoop()
                        .With(requester)
                        .WithDefaultLoader(loaderOptions);

                case ProductRetailer.BestBuy:
                    return Configuration.Default
                        .WithCulture("en-US")
                        .With(defaultRequester)
                        .WithDefaultLoader(loaderOptions)
                        .WithDefaultCookies();

                case ProductRetailer.Lowes:
                    var lowesCookieString = new Dictionary<string, string>() {
                        {"region", "east"},
                        {"dbidv2", "a184956c-a045-4833-90e1-5b2df112cda9"},
                        {"akaalb_prod_dual", "1650198217~op=PROD_GCP_EAST_CTRL_B:PROD_CTRL_B|PROD_GCP_EAST_CTRL_DFLT:PROD_DEFAULT_CTRL|~rv=3~m=PROD_CTRL_B:0|PROD_DEFAULT_CTRL:0|~os=352fb8a62db4e37e16b221fb4cefd635~id=8ef75d6cb63ba4ea4de213871a2612f5"},
                        {"akavpau_default", "1650112117~id=4b7b86c8273a6b99de7ca721ad2780b0"},
                        {"al_sess", "FuA4EWsuT07UWryyq/3foEuW2tgSpbozjhXqt9Y/xMKC5mT+HD+3p7dCkqnWql0F"},
                        {"p13n", "%7B%22zipCode%22%3A%2215120%22%2C%22storeId%22%3A%220780%22%2C%22state%22%3A%22PA%22%2C%22audienceList%22%3A%5B%22WPRO%22%5D%7D"},
                        {"sn", "0780"},
                        {"notice_behavior", "implied,eu"}
                    };
                    cookies = _requesterHelper.GetCookieString(lowesCookieString);
                    userAgent = _requesterHelper.GetRandomUserAgentString();
                    headers = _requesterHelper.GetHeaders(userAgent, cookies);
                    requester = _requesterHelper.GetRequester(headers);

                    return Configuration.Default
                        .WithJs()
                        .WithCulture("en-US")
                        .WithEventLoop()
                        .With(requester)
                        .WithDefaultLoader(loaderOptions);

                default:
                    return null;
            }
        }

        public string GetRetailerSelector(ProductRetailer retailer, string defaultSelector)
        {// TODO: provide array of selectors to try
            switch (retailer)
            {
                case ProductRetailer.Lowes:
                    return "div.newPriceWrapper div.main-price span.item-price-dollar";

                case ProductRetailer.HomeDepot:
                    return "div.price-format__large.price-format__main-price span:nth-of-type(2)";

                case ProductRetailer.BestBuy:
                    return "div.price-box div.priceView-hero-price.priceView-customer-price span:nth-of-type(1)";

                case ProductRetailer.AppliancesConnection:
                    return "span.the-price.price-amt";

                default:
                    return defaultSelector;
            }
        }
    }
}
