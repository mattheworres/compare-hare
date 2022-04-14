using System;
using AutoMapper;
using CompareHare.Api.Features.Products.Mapping;
using CompareHare.Api.Features.Products.Models;
using CompareHare.Domain.Entities;

namespace CompareHare.Api.Features.Products.GetProductCurrent
{
    public class GetProductCurrentMappingProfile : Profile
    {
        public GetProductCurrentMappingProfile()
        {
            CreateMap<TrackedProduct, ProductCurrentDisplayModel>()
                .ForMember(d => d.ProductName, mce => mce.MapFrom(s => s.Name))
                .ForMember(d => d.TrackedProductId, mce => mce.MapFrom(s => s.Id));

            CreateMap<TrackedProductRetailer, ProductRetailersListModel>()
                .ForMember(d => d.LastUpdated, mce => mce.Ignore())
                .ForMember(d => d.PriceIsManual, mce => mce.Ignore())
                .ForMember(d => d.RetailerName, mce => mce.MapFrom<RetailersNameValueResolver>())
                .ForMember(d => d.TrackedProductRetailerId, mce => mce.MapFrom(s => s.Id))
                ;

            CreateMap<ProductRetailerPrice, ProductRetailersListModel>()
                .ForMember(d => d.RetailerName, mce => mce.MapFrom(s => s.TrackedProductRetailer.ProductRetailer.ToString()))
                .ForMember(d => d.TrackedProductRetailerId, mce => mce.MapFrom(s => s.TrackedProductRetailerId))
                .ForMember(d => d.LastUpdated, mce => mce.MapFrom(s => s.PriceDate))
                .ForMember(d => d.Enabled, mce => mce.MapFrom(s => s.TrackedProductRetailer.Enabled))
                .ForMember(d => d.Price, mce => mce.MapFrom(s => s.Price))
                ;
        }
    }
}
