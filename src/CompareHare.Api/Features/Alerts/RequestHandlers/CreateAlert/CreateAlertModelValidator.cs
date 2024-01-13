
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Domain.Entities.Constants;
using FluentValidation;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.CreateAlert
{
    public class CreateAlertModelValidator : AbstractValidator<CreateAlertModel>
    {
        public CreateAlertModelValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(1, 256).WithMessage("Name can only be up to 256 characters");

            RuleFor(x => x.Zip)
                .Length(5).WithMessage("Zip code must be 5 digit format");

            RuleFor(x => x.UtilityState)
                .Must(state => Enum.IsDefined(typeof(UtilityStates), state))
                .WithMessage("Incorrect utility state value");

            RuleFor(x => x.UtilityType)
                .Must(type => Enum.IsDefined(typeof(UtilityTypes), type))
                .WithMessage("Incorrect utility type");

            //RuleSet("Prices", () => {
            RuleFor(m => m.MinimumPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.HasMinimumPrice)
                .WithMessage("Min price must be positive");

            RuleFor(x => x.MinimumPrice)
                .LessThan(x => x.MaximumPrice)
                .When(x => x.HasMinimumPrice && x.HasMaximumPrice)
                .WithMessage("Min price must be less than max price");

            RuleFor(x => x.MaximumPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.HasMinimumPrice)
                .WithMessage("Max price must be positive");

            RuleFor(x => x.MaximumMonthLength)
                .GreaterThan(x => x.MinimumMonthLength)
                .When(x => x.HasMinimumPrice && x.HasMaximumPrice)
                .WithMessage("Max price must be greater than min price");

            //RuleSet("Months", () => {
            RuleFor(m => m.MinimumMonthLength)
                .GreaterThanOrEqualTo(1)
                .When(x => x.HasMinimumMonthLength)
                .WithMessage("Min month must be at least 1");

            RuleFor(m => m.MinimumMonthLength)
                .LessThan(x => x.MaximumMonthLength)
                .When(x => x.HasMinimumMonthLength && x.HasMaximumMonthLength)
                .WithMessage("Min month must be less than max month");

            RuleFor(m => m.MaximumMonthLength)
                .GreaterThanOrEqualTo(1)
                .When(x => x.HasMaximumMonthLength)
                .WithMessage("Min month must be at least 1");

            RuleFor(m => m.MaximumMonthLength)
                .GreaterThan(x => x.MinimumMonthLength)
                .When(x => x.HasMinimumMonthLength && x.HasMaximumMonthLength)
                .WithMessage("Max month must be greater than min month");

            //RuleSet("Renewable", () => {
            RuleFor(x => x.MinimumRenewablePercent)
                .GreaterThanOrEqualTo(0)
                .When(x => x.FilterRenewable && x.HasRenewable)
                .WithMessage("Min renewable percent must be positive");

            RuleFor(x => x.MinimumRenewablePercent)
                .LessThan(x => x.MaximumRenewablePercent)
                .When(x => x.FilterRenewable && x.HasRenewable)
                .WithMessage("Min renewable percent must be less than max");

            RuleFor(x => x.MaximumRenewablePercent)
                .LessThanOrEqualTo(100)
                .GreaterThan(1)
                .When(x => x.FilterRenewable && x.HasRenewable)
                .WithMessage("Max renewable percent must be positive and less than 100");

            RuleFor(x => x.MaximumRenewablePercent)
                .GreaterThan(x => x.MinimumRenewablePercent)
                .When(x => x.FilterRenewable && x.HasRenewable)
                .WithMessage("Max renewable percent must be greater than min");

            RuleFor(x => x.Comments)
                .Length(0, 512)
                .WithMessage("Comments are optional but can only be at most 512 chars long");
        }
    }
}
