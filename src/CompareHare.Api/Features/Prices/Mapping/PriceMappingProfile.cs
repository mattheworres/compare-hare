using AutoMapper;
using CompareHare.Domain.Entities;

namespace CompareHare.Api.Features.Prices.Mapping
{
    public class PriceMappingProfile : Profile
    {
        public PriceMappingProfile() {
            CreateMap<ProductRetailerPrice, ProductRetailerPriceHistory>()
                .ForMember(d => d.Id, mce => mce.Ignore())
            ;

            CreateMap<ProductRetailerPrice, ProductRetailerPrice>()
                .ForMember(d => d.Id, mce => mce.Ignore())
                .ForMember(d => d.CreatedDate, mce => mce.Ignore())
                .ForMember(d => d.AmountChange, mce => mce.Ignore())
                .ForMember(d => d.PercentChange, mce => mce.Ignore());
        }
    }
}
