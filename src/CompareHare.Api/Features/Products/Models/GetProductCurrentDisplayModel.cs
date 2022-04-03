namespace CompareHare.Api.Features.Products.Models
{
    public class GetProductCurrentDisplayModel
    {
        public GetProductCurrentDisplayModel(int trackedProductId) {
            TrackedProductId = trackedProductId;
        }

        public int TrackedProductId { get; }
    }
}