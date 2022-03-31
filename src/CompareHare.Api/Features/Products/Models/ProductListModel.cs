using System.Collections.Generic;

namespace CompareHare.Api.Features.Products.Models
{
    public class ProductListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Enabled { get; set; }
        public IEnumerable<string> Retailers { get; set; }
    }
}
