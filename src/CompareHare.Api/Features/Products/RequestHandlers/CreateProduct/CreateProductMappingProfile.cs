using AutoMapper;
using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.Features.Shared.Mapping;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Entities.Constants;

namespace CompareHare.Api.Features.Products.RequestHandlers.CreateProduct
{
    public class CreateProductMappingProfile : Profile
    {
        public CreateProductMappingProfile() {
            CreateMap<CreateProductRetailerModel, TrackedProductRetailer>()
                .ForMember(d => d.Id, mce => mce.Ignore())
                .ForMember(d => d.ProductRetailer, mce => mce.MapFrom(s => (ProductRetailer)s.ProductRetailer))
            ;

            CreateMap<CreateProductModel, TrackedProduct>()
                .ForMember(d => d.UserId, mce => mce.MapFrom<CurrentUserIdResolver>())
                .ForMember(d => d.Id, mce => mce.Ignore())
            ;
        }
    }
}
