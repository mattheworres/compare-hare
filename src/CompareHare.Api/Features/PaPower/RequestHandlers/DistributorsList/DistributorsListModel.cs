namespace CompareHare.Api.Features.PaPower.RequestHandlers.DistributorsList
{
    public class DistributorsListModel
    {
        public DistributorsListModel(string zipCode) {
            ZipCode = zipCode;
        }

        public string ZipCode { get; set; }
    }
}
