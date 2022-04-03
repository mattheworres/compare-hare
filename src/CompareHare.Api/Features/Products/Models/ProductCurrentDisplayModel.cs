using System.Collections.Generic;

namespace CompareHare.Api.Features.Products.Models
{
    public class ProductCurrentDisplayModel
    {
        public int TrackedProductId { get; set; }
        public string ProductName { get; set; }
        public bool Enabled { get; set; }

        public IEnumerable<ProductRetailersListModel> ProductRetailers { get; set; }
    }
}
