using AutoMapper;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.Features.Shared.Mapping;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Entities.Constants;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.CreateAlert
{
    public class CreateAlertMappingProfile : Profile
    {
        public CreateAlertMappingProfile() {
            CreateMap<CreateAlertModel, StateUtilityIndex>()
                .ForMember(d => d.Id, mce => mce.Ignore())
                .ForMember(d => d.UtilityState, mce => mce.MapFrom(s => (UtilityStates)s.UtilityState))
                .ForMember(d => d.UtilityType, mce => mce.MapFrom(s => (UtilityTypes)s.UtilityType))
            ;

            CreateMap<CreateAlertModel, Alert>()
                .ForMember(d => d.Active, mce => mce.MapFrom(x => true))
                .ForMember(d => d.UserId, mce => mce.MapFrom<CurrentUserIdResolver>())
                .ForMember(d => d.MinimumPrice, mce => mce.MapFrom(s => s.HasMinimumPrice ? (decimal?)s.MinimumPrice : null))
                .ForMember(d => d.MaximumPrice, mce => mce.MapFrom(s => s.HasMaximumPrice ? (decimal?)s.MaximumPrice : null))
                .ForMember(d => d.MinimumRenewablePercent, mce => mce.MapFrom(s => s.FilterRenewable && s.HasRenewable ? (decimal?)s.MinimumRenewablePercent : null))
                .ForMember(d => d.MaximumRenewablePercent, mce => mce.MapFrom(s => s.FilterRenewable && s.HasRenewable ? (decimal?)s.MaximumRenewablePercent : null))
                .ForMember(d => d.HasCancellationFee, mce => mce.MapFrom(s => s.FilterCancellationFee ? (bool?)s.HasCancellationFee : null))
                .ForMember(d => d.HasMonthlyFee, mce => mce.MapFrom(s => s.FilterMonthlyFee ? (bool?)s.HasMonthlyFee : null))
                .ForMember(d => d.HasEnrollmentFee, mce => mce.MapFrom(s => s.FilterEnrollmentFee ? (bool?)s.HasEnrollmentFee : null))
                .ForMember(d => d.RequiresDeposit, mce => mce.MapFrom(s => s.FilterRequiresDeposit ? (bool?)s.RequiresDeposit : null))
                .ForMember(d => d.HasBulkDiscounts, mce => mce.MapFrom(s => s.FilterBulkDiscounts ? (bool?)s.HasBulkDiscounts : null))
                //here, map teh models hopefully with just straight MapFrom's and not crazy resolvers everywhere
            ;
        }
    }
}
