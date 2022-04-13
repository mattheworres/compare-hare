using System;
using CompareHare.Domain.Services;
using CompareHare.Domain.Entities.Constants;

namespace CompareHare.Domain.Entities
{
    public class ProductRetailerPrice : ICreatedDateTimeTracker, IModifiedDateTimeTracker
    {
        public int Id { get; set; }

        public int TrackedProductId { get; set; }
        public TrackedProduct TrackedProduct { get; set; }

        public int TrackedProductRetailerId { get; set; }
        public TrackedProductRetailer TrackedProductRetailer { get; set; }

        // Search the Prices table, but use the history ID to never break constraints in the future
        public int ProductRetailerPriceHistoryId { get; set; }
        public virtual ProductRetailerPriceHistory ProductRetailerPriceHistory { get; set; }

        public ProductRetailer ProductRetailer { get; set; }

        public float? Price { get; set; }

        public float? AmountChange { get; set; }
        public float? PercentChange { get; set; }

        // TODO: like, AppliancesConnection hiding the "true" price
        public bool HasScrapingFootnote { get; set; }
        public string Footnote { get; set; }

        public bool PriceIsManual { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
