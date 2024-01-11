namespace CompareHare.Api.Features.Offers.RequestHandlers.Populate
{
    public class PopulateModel
    {
        public PopulateModel(int alertId) {
            AlertId = alertId;
        }

        public int AlertId { get; set; }
    }
}
