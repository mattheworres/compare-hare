using System.Collections.Generic;

namespace CompareHare.Api.Features.Products.Models
{
    public class CreateProductModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<CreateProductRetailerModel> ProductRetailers { get; set; }
    }
}
