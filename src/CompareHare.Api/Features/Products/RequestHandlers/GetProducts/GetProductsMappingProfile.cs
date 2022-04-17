using AutoMapper;
using CompareHare.Api.Features.Products.Models;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Extensions;

namespace CompareHare.Api.Features.Products.RequestHandlers.GetProducts
{
    public class GetProductsMappingProfile : Profile
    {
        public GetProductsMappingProfile()
        {
            CreateMap<TrackedProduct, ProductListModel>();

            CreateMap<ProductRetailerPrice, ProductListModel>()
                .ForMember(d => d.Id, mce => mce.Ignore())
                .ForMember(d => d.LowPriceRetailerName, mce => mce.MapFrom(s => s.ProductRetailer.DisplayName()))
                .ForMember(d => d.PriceLastUpdated, mce => mce.MapFrom(s => s.PriceDate))
            ;
        }
    }
}
