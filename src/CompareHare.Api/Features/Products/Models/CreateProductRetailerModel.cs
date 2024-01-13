namespace CompareHare.Api.Features.Products.Models
{
    public class CreateProductRetailerModel
    {
        public CreateProductRetailerModel()
        {
            OtherRetailerDisplayName = "";
            ScrapeUrl = "";
            PriceSelector = "";
        }

        public int? Id { get; set; }
        public int? TrackedProductId { get; set; }
        public int ProductRetailer { get; set; }
        public string OtherRetailerDisplayName { get; set; }


        public string ScrapeUrl { get; set; }
        public string PriceSelector { get; set; }
    }
}
