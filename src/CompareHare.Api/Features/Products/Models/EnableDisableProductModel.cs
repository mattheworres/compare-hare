using System;

namespace CompareHare.Api.Features.Products.Models
{
    public class EnableDisableProductModel
    {
        public EnableDisableProductModel(int trackedProductId, bool enabled)
        {
            TrackedProductId = trackedProductId;
            Enabled = enabled;
        }

        public int TrackedProductId { get; }
        public bool Enabled { get; }
    }
}
