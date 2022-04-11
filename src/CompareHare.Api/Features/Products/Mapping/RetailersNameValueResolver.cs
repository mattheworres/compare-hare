using AutoMapper;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Extensions;

namespace CompareHare.Api.Features.Products.Mapping
{
    public class RetailersNameValueResolver : IValueResolver<TrackedProductRetailer, object, string>
    {
        public string Resolve(TrackedProductRetailer source, object destination, string destMember, ResolutionContext context)
        {
            return source.ProductRetailer.DisplayName();
        }
    }
}
