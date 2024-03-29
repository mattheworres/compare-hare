using AutoMapper;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.Features.Shared.Mapping;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Extensions;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.GetAlert
{
    public class GetAlertMappingProfile : Profile
    {
        public GetAlertMappingProfile()
        {
            CreateMap<Alert, AlertDisplayModel>()
                .ForMember(d => d.UtilityState, mce => mce.MapFrom(s => s.StateUtilityIndex.UtilityState.DisplayName()))
                .ForMember(d => d.UtilityType, mce => mce.MapFrom(s => s.StateUtilityIndex.UtilityType.DisplayName()))
                .ForMember(d => d.LoaderIdentifier, mce => mce.MapFrom(s => s.StateUtilityIndex.LoaderDataIdentifier))
                .ForMember(d => d.LoaderIdentifier2, mce => mce.MapFrom(s => s.StateUtilityIndex.LoaderDataIdentifier2))
                .ForMember(d => d.LoaderIdentifier3, mce => mce.MapFrom(s => s.StateUtilityIndex.LoaderDataIdentifier3))
                .ForMember(d => d.MatchesCount, mce => mce.MapFrom<AlertMatchCountResolver>())
                .ForMember(d => d.Parameters, mce => mce.MapFrom<AlertFeatureResolver>())
                .ForMember(d => d.Prices, mce => mce.Ignore())
                .ForMember(d => d.LastEdited, mce => mce.MapFrom(s => s.ModifiedDate))
            ;

            CreateMap<UtilityPriceHistory, PriceDisplayModel>();
        }
    }
}
