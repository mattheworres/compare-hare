using CompareHare.Api.Features.Shared.Validation.Interfaces;
using FluentValidation;

namespace CompareHare.Api.Features.PaPower.RequestHandlers.DistributorsList
{
    public class DistributorsListModelValidator : DefaultValidatorInterceptor<DistributorsListModel>
    {
        public DistributorsListModelValidator()
        {
            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .WithMessage("Zip code required")
                .Length(5).WithMessage("Zip code must be 5 digit format"); ;
        }
    }
}
