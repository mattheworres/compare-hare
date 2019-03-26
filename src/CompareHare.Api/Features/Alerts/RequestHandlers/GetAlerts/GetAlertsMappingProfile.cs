using System.Linq;
using AutoMapper;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Extensions;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.GetAlerts
{
    public class GetAlertsMappingProfile : Profile
    {
        public GetAlertsMappingProfile() {
            CreateMap<Alert, AlertListModel>()
                .ForMember(d => d.LastEdited, mce => mce.MapFrom(s => s.ModifiedDate))
                .ForMember(d => d.MatchesCount, mce => mce.Ignore())
                .ForMember(d => d.UtilityState, mce => mce.MapFrom(s => s.StateUtilityIndex.UtilityState.DisplayName()))
                .ForMember(d => d.UtilityType, mce => mce.MapFrom(s => s.StateUtilityIndex.UtilityType.DisplayName()))
                .ForMember(d => d.MatchesCount, mce => mce.MapFrom(s => s.AlertMatch.UtilityPriceHistories.Count()))
                ;

            CreateMap<AlertMatch, AlertListModel>()
                .ForMember(d => d.Id, mce => mce.Ignore())
                .ForMember(d => d.MatchesCount, mce => mce.MapFrom(s => s.UtilityPriceHistories.Count()))
                ;
        }
    }
}
