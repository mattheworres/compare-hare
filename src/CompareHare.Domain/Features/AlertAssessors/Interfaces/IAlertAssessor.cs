using System.Threading.Tasks;
using CompareHare.Domain.Features.AlertAssessors.Models;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Domain.Features.AlertAssessors.Interfaces
{
    public interface IAlertAssessor : IFeatureService
    {
        Task<AlertAssessorReturnModel> AssessMatches(int alertId);
    }
}
