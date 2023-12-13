using System;

namespace CompareHare.Api.Features.Prices.Models
{
    public class AddManualPriceModel
    {
        public int TrackedProductRetailerId { get; set; }
        public float Price { get; set; }
        public DateTimeOffset PriceDate { get; set; }
    }
}