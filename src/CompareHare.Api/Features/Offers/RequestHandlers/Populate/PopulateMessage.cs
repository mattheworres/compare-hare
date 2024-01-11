using CompareHare.Api.Features.Shared.Validation.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Offers.RequestHandlers.Populate
{
    public class PopulateMessage : IRequest<IActionResult>, IValidatableRequest<PopulateModel>
    {
        public PopulateMessage(PopulateModel model) {
            Model = model;
        }

        public PopulateModel Model { get; set; }
    }
}
