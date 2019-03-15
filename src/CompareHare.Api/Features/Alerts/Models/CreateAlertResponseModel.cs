namespace CompareHare.Api.Features.Alerts.Models
{
    public class CreateAlertResponseModel
    {
        public CreateAlertResponseModel(bool wasCreatedOrUpdated, int alertId, int matches = 0) {
            IndexWasCreatedOrUpdated = wasCreatedOrUpdated;
            AlertId = alertId;
            MatchesFound = matches;
        }

        public bool IndexWasCreatedOrUpdated { get; set; }
        public int AlertId { get; set; }
        public int MatchesFound { get; set; }
    }
}
