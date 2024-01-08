#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using CompareHare.Domain.Services;
using CompareHare.Domain.Entities.Constants;

namespace CompareHare.Domain.Entities
{
    public class ProductRetailerPriceHistory : ICreatedDateTimeTracker, IModifiedDateTimeTracker
    {
        public int Id { get; set; }

        public int TrackedProductId { get; set; }
        public TrackedProduct TrackedProduct { get; set; }

        public int TrackedProductRetailerId { get; set; }
        public TrackedProductRetailer TrackedProductRetailer { get; set; }

        public ProductRetailer ProductRetailer { get; set; }

        public float? Price { get; set; }

        public float? AmountChange { get; set; }
        public float? PercentChange { get; set; }

        // TODO: like, AppliancesConnection hiding the "true" price
        public bool HasScrapingFootnote { get; set; }
        public string Footnote { get; set; }

        public bool PriceIsManual { get; set; }

        public DateTimeOffset PriceDate { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
