using System;
using CompareHare.Domain.Services;
using CompareHare.Domain.Entities.Constants;

namespace CompareHare.Domain.Entities
{
    public class ProductRetailerPriceHistory : ICreatedDateTimeTracker, IModifiedDateTimeTracker
    {
        public int Id { get; set; }

        public int TrackedProductId { get; set; }
        public TrackedProduct TrackedProduct { get; set; }

        public ProductRetailer ProductRetailer { get; set; }

        public float? Price { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
