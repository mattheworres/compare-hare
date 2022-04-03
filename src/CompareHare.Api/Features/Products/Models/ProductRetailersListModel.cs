using System;

namespace CompareHare.Api.Features.Products.Models
{
    public class ProductRetailersListModel
    {
        public int TrackedProductId { get; set; }
        public int TrackedProductRetailerId { get; set; }
        public string RetailerName { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public bool Enabled { get; set; }
        public bool PriceIsManual { get; set; }
        public bool HasScrapingFootnote { get; set; }
        public string Footnote { get; set; }

        public float? Price { get; set; }
        public float? AmountChange { get; set; }
        public float? PercentChange { get; set; }
    }
}
