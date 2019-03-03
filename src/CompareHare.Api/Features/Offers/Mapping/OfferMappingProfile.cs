using AutoMapper;
using CompareHare.Api.Features.Offers.Models;
using CompareHare.Domain.Entities;

namespace CompareHare.Api.Features.Offers.Mapping
{
    public class OfferMappingProfile : Profile
    {
        public OfferMappingProfile() {
            CreateMap<UtilityPrice, UtilityPriceHashModel>()
            ;

            CreateMap<UtilityPrice, UtilityPriceHistory>()
                .ForMember(d => d.Id, mce => mce.Ignore())
            ;
        }
    }
}
