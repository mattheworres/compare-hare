#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations;

namespace CompareHare.Domain.Entities
{
    public class TrackedProduct
    {
        public TrackedProduct()
        {
            Name = "";
        }

        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required, MaxLength(512)]
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public virtual IEnumerable<TrackedProductRetailer> Retailers { get; set; }
        public virtual IEnumerable<ProductRetailerPrice> Prices { get; set; }
        public virtual IEnumerable<ProductRetailerPriceHistory> PriceHistories { get; set; }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
