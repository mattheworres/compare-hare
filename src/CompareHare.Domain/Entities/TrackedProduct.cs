using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompareHare.Domain.Entities
{
    public class TrackedProduct
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required, MaxLength(512)]
        public string Name { get; set; }

        public virtual IEnumerable<TrackedProductRetailer> Retailers { get; set; }
        public virtual IEnumerable<ProductRetailerPriceHistory> PriceHistories { get; set; }
    }
}
