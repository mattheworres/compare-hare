using CompareHare.Api.Features.Shared.Models;

namespace CompareHare.Api.Features.Products.Models
{
    public class GetCreateProductModel
    {
        public GetCreateProductModel(IEnumerable<SelectListOptionModel> retailers)
        {
            Retailers = retailers;
        }

        public IEnumerable<SelectListOptionModel> Retailers { get; set; }
    }
}
