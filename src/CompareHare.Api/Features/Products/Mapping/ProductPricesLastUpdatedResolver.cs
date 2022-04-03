using System;
using System.Linq;
using AutoMapper;
using CompareHare.Domain.Entities;

namespace CompareHare.Api.Features.Products.Mapping
{
    public class ProductPricesLastUpdatedResolver : IValueResolver<TrackedProductRetailer, object, DateTimeOffset>
    {
        public DateTimeOffset Resolve(TrackedProductRetailer source, object destination, DateTimeOffset destMember, ResolutionContext context)
        {
            return source.ProductRetailerPrices
                .OrderByDescending(x => x.ModifiedDate).Select(x => x.ModifiedDate).First().Value;
        }
    }
}
