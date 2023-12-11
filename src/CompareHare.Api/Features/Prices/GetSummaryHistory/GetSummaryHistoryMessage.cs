using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Prices.GetSummaryHistory
{
    public class GetSummaryHistoryMessage : IRequest<IActionResult>
    {
        public GetSummaryHistoryMessage() {
            
        }
    }
}