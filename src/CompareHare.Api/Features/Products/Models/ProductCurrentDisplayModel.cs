namespace CompareHare.Api.Features.Products.Models
{
    public class ProductCurrentDisplayModel
    {
        public ProductCurrentDisplayModel()
        {
            ProductRetailers = new List<ProductRetailersListModel>();
            ProductName = "";
        }

        public int TrackedProductId { get; set; }
        public string ProductName { get; set; }
        public bool Enabled { get; set; }

        public List<ProductRetailersListModel> ProductRetailers { get; set; }
    }
}
