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

        [Required, MaxLength(512)]
        public string ScrapeUrl { get; set; }

        [Required, MaxLength(128)]
        public string PriceSelector { get; set; }
    }
}
