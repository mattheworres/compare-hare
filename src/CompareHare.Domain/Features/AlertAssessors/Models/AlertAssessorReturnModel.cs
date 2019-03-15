using System.Collections.Generic;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Entities.Constants;

namespace CompareHare.Domain.Features.AlertAssessors.Models
{
    public class AlertAssessorReturnModel
    {
        public AlertAssessorReturnType ReturnType { get; set; }
        public List<UtilityPriceHistory> UpdatedMatches { get; set; }
    }
}
