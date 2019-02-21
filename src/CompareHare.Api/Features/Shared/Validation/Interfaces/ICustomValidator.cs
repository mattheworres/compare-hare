#region usings

using System.Threading.Tasks;

#endregion

namespace CompareHare.Api.Features.Shared.Validation.Interfaces
{
    public interface ICustomValidator<in TModel>
    {
        Task<CustomValidationFailures> ValidateAsync(TModel model);
    }
}
