using CompareHare.Api.Features.Prices.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Prices.CheckPriceDate
{
    public class CheckPriceDateMessage : IRequest<IActionResult>
    {
        public CheckPriceDateMessage(CheckPriceDateModel model)
        {
            Model = model;
        }

        public CheckPriceDateModel Model { get; }
    }
}