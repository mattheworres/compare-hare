

namespace CompareHare.Api.Features.Alerts.Models
{
    public class AlertListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UtilityState { get; set; }
        public string UtilityType { get; set; }

        public int MatchesCount { get; set; }

        public DateTime LastEdited { get; set; }
    }
}
