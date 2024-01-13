namespace CompareHare.Api.Features.Products.Models
{
    public class ProductListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ProductListModel(string name)
        {
            Name = name;
            LowPriceRetailerName = "";
        }

        public bool Enabled { get; set; }

        // Represents lowest price, regardless of
        public string LowPriceRetailerName { get; set; }
        public DateTimeOffset? PriceLastUpdated { get; set;}
        public float? Price { get; set; }
        public float? AmountChange { get; set; }
        public float? PercentChange { get; set; }
        public bool PriceIsManual { get; set; }
        public bool HasScrapingFootnote { get; set; }
        public bool HasScrapingExceptions { get; set; }
    }
}
