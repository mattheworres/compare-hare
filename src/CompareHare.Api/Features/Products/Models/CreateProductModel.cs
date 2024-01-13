namespace CompareHare.Api.Features.Products.Models
{
    public class CreateProductModel
    {
        public CreateProductModel()
        {
            Name = "";
            ProductRetailers = new List<CreateProductRetailerModel>();
        }

        public int? Id { get; set; }

        public string Name { get; set; }


        public IEnumerable<CreateProductRetailerModel> ProductRetailers { get; set; }
    }
}
