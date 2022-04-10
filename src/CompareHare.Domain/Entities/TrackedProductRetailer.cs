using System.ComponentModel.DataAnnotations;
using CompareHare.Domain.Entities.Constants;
using System.Collections.Generic;

namespace CompareHare.Domain.Entities
{
    public class TrackedProductRetailer
    {
        public int Id { get; set; }

        public int TrackedProductId { get; set; }
        public TrackedProduct TrackedProduct { get; set; }

        public bool Enabled { get; set; }

        public ProductRetailer ProductRetailer { get; set; }

        public string OtherRetailerDisplayName { get; set; }

        public virtual IEnumerable<ProductRetailerPrice> ProductRetailerPrices { get; set; }
        public virtual IEnumerable<ProductRetailerPriceHistory> ProductRetailerPriceHistories { get; set; }

        [Required, MaxLength(512)]
        public string ScrapeUrl { get; set; }

        [MaxLength(128)]
        public string PriceSelector { get; set; }
    }
}
