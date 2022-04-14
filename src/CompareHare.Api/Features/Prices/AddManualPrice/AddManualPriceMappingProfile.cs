using AutoMapper;
using CompareHare.Api.Features.Prices.Models;
using CompareHare.Domain.Entities;

namespace CompareHare.Api.Features.Prices.AddManualPrice
{
    public class AddManualPriceMappingProfile : Profile
    {
        public AddManualPriceMappingProfile()
        {
            CreateMap<AddManualPriceModel, ProductRetailerPriceHistory>()
                .ForMember(d => d.CreatedDate, mce => mce.Ignore())
                .ForMember(d => d.ModifiedDate, mce => mce.Ignore())
                .ForMember(d => d.PriceIsManual, mce => mce.MapFrom(s => true))
                ;

            CreateMap<TrackedProductRetailer, ProductRetailerPriceHistory>()
                .ForMember(d => d.Id, mce => mce.Ignore())
                .ForMember(d => d.CreatedDate, mce => mce.Ignore())
                .ForMember(d => d.ModifiedDate, mce => mce.Ignore())
                ;

            CreateMap<ProductRetailerPriceHistory, ProductRetailerPrice>()
                .ForMember(d => d.Id, mce => mce.Ignore())
                .ForMember(d => d.ProductRetailerPriceHistory, mce => mce.MapFrom(s => s))
            ;
        }
    }
}