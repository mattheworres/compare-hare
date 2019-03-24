using AutoMapper;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Services.Models;

namespace CompareHare.Api.Features.Offers.Mapping
{
    public class OfferMappingProfile : Profile
    {
        public OfferMappingProfile() {
            CreateMap<UtilityPrice, UtilityPriceHashModel>()
            ;

            CreateMap<UtilityPriceHistory, UtilityPriceHashModel>()
            ;

            CreateMap<UtilityPrice, UtilityPriceHistory>()
                .ForMember(d => d.Id, mce => mce.Ignore())
            ;
        }
    }
}
