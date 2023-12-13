namespace CompareHare.Api.Features.Products.Models
{
    public class EnableDisableProductRetailerModel
    {
        public EnableDisableProductRetailerModel(int trackedProductRetailerId, bool enabled)
        {
            TrackedProductRetailerId = trackedProductRetailerId;
            Enabled = enabled;
        }

        public int TrackedProductRetailerId { get; }
        public bool Enabled { get; }
    }
}
