using System.Collections.Generic;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Services.Models;

namespace CompareHare.Domain.Features.AlertAssessors.Models
{
    public class AlertAssessorReturnModel
    {
        public AlertAssessorReturnType ReturnType { get; set; }
        public IEnumerable<UtilityPriceHashModel> UpdatedMatches { get; set; }
    }
}
