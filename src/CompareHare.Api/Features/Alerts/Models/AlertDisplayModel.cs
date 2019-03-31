using System;
using System.Collections.Generic;

namespace CompareHare.Api.Features.Alerts.Models
{
    public class AlertDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UtilityState { get; set; }
        public string UtilityType { get; set; }
        public string LoaderIdentifier { get; set; }

        public int MatchesCount { get; set; }

        public IEnumerable<AlertDisplayParameter> Parameters { get; set; }
        public List<PriceDisplayModel> Prices { get; set; }

        public DateTime LastEdited { get; set; }
    }
}
