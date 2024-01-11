using CompareHare.Api.Features.Prices.Models;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Prices.AddManualPrice
{
    public class AddManualPriceMessage : IRequest<IActionResult>, IValidatableRequest<AddManualPriceModel>
    {
        public AddManualPriceModel Model { get; }

        public AddManualPriceMessage(AddManualPriceModel model)
        {
            Model = model;
        }
    }
}