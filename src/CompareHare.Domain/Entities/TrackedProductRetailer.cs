#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations;
using CompareHare.Domain.Entities.Constants;


namespace CompareHare.Domain.Entities
{
    public class TrackedProductRetailer
    {
        public TrackedProductRetailer()
        {
            OtherRetailerDisplayName = "";
            ScrapeUrl = "";
            PriceSelector = "";
        }

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

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
