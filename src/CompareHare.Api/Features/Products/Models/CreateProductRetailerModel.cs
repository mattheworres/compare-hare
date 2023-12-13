using CompareHare.Domain.Entities.Constants;

namespace CompareHare.Api.Features.Products.Models
{
    public class CreateProductRetailerModel
    {
        public int? Id { get; set; }
        public int? TrackedProductId { get; set; }
        public int ProductRetailer { get; set; }
        public string OtherRetailerDisplayName { get; set; }
        public string ScrapeUrl { get; set; }
        public string PriceSelector { get; set; }
    }
}
