using System.Linq;
using AutoMapper;
using CompareHare.Domain.Entities;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.GetAlert
{
    public class AlertMatchCountResolver : IValueResolver<Alert, object, int>
    {
        public int Resolve(Alert source, object destination, int destMember, ResolutionContext context)
        {
            if (source.AlertMatch == null || source.AlertMatch.UtilityPriceHistories.Count() == 0) return 0;

            return source.AlertMatch.UtilityPriceHistories.Count();
        }
    }
}
