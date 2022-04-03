using System;
using CompareHare.Domain.Services;
using CompareHare.Domain.Entities.Constants;

namespace CompareHare.Domain.Entities
{
    public class ProductPriceScrapingException : ICreatedDateTimeTracker
    {
        public int Id { get; set; }

        public int TrackedProductId { get; set; }
        public TrackedProduct TrackedProduct { get; set; }

        public int TrackedProductRetailerId { get; set; }
        public TrackedProductRetailer TrackedProductRetailer { get; set; }

        public ProductRetailer ProductRetailer { get; set; }

        public string Url { get; set; }
        public string Selector { get; set; }

        public string Error { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
