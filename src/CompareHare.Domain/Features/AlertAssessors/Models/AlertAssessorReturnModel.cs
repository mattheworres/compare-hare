using CompareHare.Domain.Entities.Constants;

namespace CompareHare.Domain.Features.AlertAssessors.Models
{
    public class AlertAssessorReturnModel
    {
        public AlertAssessorReturnType ReturnType { get; set; }
        public int MatchesCount { get; set; }
    }
}
