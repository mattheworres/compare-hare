using AutoMapper;
using CompareHare.Api.Features.Products.Mapping;
using CompareHare.Api.Features.Products.Models;
using CompareHare.Domain.Entities;

namespace CompareHare.Api.Features.Products.RequestHandlers.GetProducts
{
    public class GetProductsMappingProfile : Profile
    {
        public GetProductsMappingProfile()
        {
            CreateMap<TrackedProduct, ProductListModel>()
                .ForMember(d => d.Retailers, mce => mce.MapFrom<ProductRetailersNameValueResolver>())
            ;
        }
    }
}
