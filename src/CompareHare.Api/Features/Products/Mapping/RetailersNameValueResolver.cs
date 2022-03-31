using System.Collections.Generic;
using AutoMapper;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Extensions;

namespace CompareHare.Api.Features.Products.Mapping
{
    public class RetailersNameValueResolver : IValueResolver<TrackedProduct, object, IEnumerable<string>>
    {
        public IEnumerable<string> Resolve(TrackedProduct source, object destination, IEnumerable<string> destMember, ResolutionContext context)
        {
            var retailerNames = new List<string>();

            foreach (var retailer in source.Retailers) {
                if (retailer.ProductRetailer != ProductRetailer.Other) {
                    retailerNames.Add(retailer.ProductRetailer.DisplayName());
                } else {
                    retailerNames.Add(retailer.OtherRetailerDisplayName);
                }
            }

            return retailerNames;
        }
    }
}
