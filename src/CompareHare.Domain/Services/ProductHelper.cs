using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Services.Interfaces;

namespace CompareHare.Domain.Services
{
    public class ProductHelper : IProductHelper
    {
        public string GetRetailerSelector(ProductRetailer retailer)
        {
            switch (retailer)
            {
                case ProductRetailer.Lowes:
                    return "span.aPrice span:nth-of-type(2)";

                case ProductRetailer.HomeDepot:
                    return "div.price-format__large.price-format__main-price span:nth-of-type(2)";

                case ProductRetailer.BestBuy:
                    return "div.price-box div.priceView-hero-price.priceView-customer-price span:nth-of-type(1)";

                case ProductRetailer.AppliancesConnection:
                    return "span.the-price.price-amt";

                default:
                    return null;
            }
        }
    }
}