namespace CompareHare.Api.Features.Alerts.Models
{
    public class GetAlertModel
    {
        public GetAlertModel(int alertId)
        {
            AlertId = alertId;
        }

        public int AlertId { get; }
    }
}
