using AutoMapper;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Services.Models;

namespace CompareHare.Domain.Services.Mapping
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile() {
            CreateMap<UtilityPrice, UtilityPriceHashModel>()
            ;

            CreateMap<UtilityPriceHistory, UtilityPriceHashModel>()
            ;

            CreateMap<UtilityPrice, UtilityPriceHistory>()
                .ForMember(d => d.Id, mce => mce.Ignore())
            ;

            CreateMap<UtilityPriceHistory, AlertMatchUtilityPriceHistory>()
                .ForMember(d => d.UtilityPriceHistoryId, mce => mce.MapFrom(s => s.Id))
            ;
        }
    }
}
