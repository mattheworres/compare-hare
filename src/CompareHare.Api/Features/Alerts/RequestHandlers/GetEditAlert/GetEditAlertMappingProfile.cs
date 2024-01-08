using AutoMapper;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Domain.Entities;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.GetEditAlert
{
  public class GetEditAlertMappingProfile : Profile
  {
    public GetEditAlertMappingProfile()
    {
        CreateMap<Alert, EditAlertModel>()
            .ForMember(d => d.HasMinimumPrice, mce => mce.MapFrom(s => s.MinimumPrice.HasValue))
            .ForMember(d => d.MinimumPrice, mce => mce.MapFrom(s => s.MinimumPrice.HasValue ? s.MinimumPrice.Value : 0))
            .ForMember(d => d.HasMaximumPrice, mce => mce.MapFrom(s => s.MaximumPrice.HasValue))
            .ForMember(d => d.MaximumPrice, mce => mce.MapFrom(s => s.MaximumPrice.HasValue ? s.MaximumPrice.Value : 0))
            .ForMember(d => d.HasMinimumMonthLength, mce => mce.MapFrom(s => s.MinimumMonthLength.HasValue))
            .ForMember(d => d.MinimumMonthLength, mce => mce.MapFrom(s => s.MinimumMonthLength.HasValue ? s.MinimumMonthLength.Value : 0))
            .ForMember(d => d.HasMaximumMonthLength, mce => mce.MapFrom(s => s.MaximumMonthLength.HasValue))
            .ForMember(d => d.MaximumMonthLength, mce => mce.MapFrom(s => s.MaximumMonthLength.HasValue ? s.MaximumMonthLength.Value : 0))
            .ForMember(d => d.FilterRenewable, mce => mce.MapFrom(s => s.HasRenewable.HasValue))
            .ForMember(d => d.HasRenewable, mce => mce.MapFrom(s => s.HasRenewable.HasValue && s.HasRenewable.Value))
            .ForMember(d => d.MinimumRenewablePercent, mce => mce.MapFrom(s => s.MinimumRenewablePercent.HasValue ? s.MinimumRenewablePercent.Value : 0))
            .ForMember(d => d.MaximumRenewablePercent, mce => mce.MapFrom(s => s.MaximumRenewablePercent.HasValue ? s.MaximumRenewablePercent.Value : 0))
            .ForMember(d => d.FilterCancellationFee, mce => mce.MapFrom(s => s.HasCancellationFee.HasValue))
            .ForMember(d => d.HasCancellationFee, mce => mce.MapFrom(s => s.HasCancellationFee.HasValue && s.HasCancellationFee.Value))
            .ForMember(d => d.FilterMonthlyFee, mce => mce.MapFrom(s => s.HasMonthlyFee.HasValue))
            .ForMember(d => d.HasMonthlyFee, mce => mce.MapFrom(s => s.HasMonthlyFee.HasValue && s.HasMonthlyFee.Value))
            .ForMember(d => d.FilterEnrollmentFee, mce => mce.MapFrom(s => s.HasEnrollmentFee.HasValue))
            .ForMember(d => d.HasEnrollmentFee, mce => mce.MapFrom(s => s.HasEnrollmentFee.HasValue && s.HasEnrollmentFee.Value))
            .ForMember(d => d.FilterNetMetering, mce => mce.MapFrom(s => s.HasNetMetering.HasValue))
            .ForMember(d => d.HasNetMetering, mce => mce.MapFrom(s => s.HasNetMetering.HasValue && s.HasNetMetering.Value))
            .ForMember(d => d.FilterRequiresDeposit, mce => mce.MapFrom(s => s.RequiresDeposit.HasValue))
            .ForMember(d => d.RequiresDeposit, mce => mce.MapFrom(s => s.RequiresDeposit.HasValue && s.RequiresDeposit.Value))
            .ForMember(d => d.FilterBulkDiscounts, mce => mce.MapFrom(s => s.HasBulkDiscounts.HasValue))
            .ForMember(d => d.HasBulkDiscounts, mce => mce.MapFrom(s => s.HasBulkDiscounts.HasValue && s.HasBulkDiscounts.Value))
        ;
    }
  }
}
