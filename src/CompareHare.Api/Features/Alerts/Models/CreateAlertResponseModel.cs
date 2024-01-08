namespace CompareHare.Api.Features.Alerts.Models
{
    public class CreateAlertResponseModel
    {
        public CreateAlertResponseModel(bool wasCreatedOrUpdated, int alertId, int? assessorResponseType = null, int matchesCount = 0) {
            IndexWasCreatedOrUpdated = wasCreatedOrUpdated;
            AlertId = alertId;
            MatchesCount = matchesCount;
            AssessorResponseType = assessorResponseType;
        }

        public bool IndexWasCreatedOrUpdated { get; set; }
        public int AlertId { get; set; }
        public int? AssessorResponseType { get; set; }
        public int MatchesCount { get; set; }
    }
}
