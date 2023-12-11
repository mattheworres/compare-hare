using System.Collections.Generic;

namespace CompareHare.Api.Features.Products.Models
{
    public class ProductCurrentDisplayModel
    {
        public ProductCurrentDisplayModel()
        {
            ProductRetailers = new List<ProductRetailersListModel>();
        }

        public int TrackedProductId { get; set; }
        public string ProductName { get; set; }
        public bool Enabled { get; set; }

        public List<ProductRetailersListModel> ProductRetailers { get; set; }
    }
}
