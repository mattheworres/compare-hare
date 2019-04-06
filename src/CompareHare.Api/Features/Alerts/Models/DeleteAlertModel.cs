namespace CompareHare.Api.Features.Alerts.Models
{
  public class DeleteAlertModel
  {
    public DeleteAlertModel(int alertId)
    {
      AlertId = alertId;
    }

    public int AlertId { get; }
  }
}
