using CompareHare.Api.Features.Shared.Validation.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.PaPower.RequestHandlers.DistributorsList
{
    public class DistributorsListMessage : IRequest<IActionResult>, IValidatableRequest<DistributorsListModel>
    {
        public DistributorsListMessage(DistributorsListModel model) {
            Model = model;
        }

        public DistributorsListModel Model { get; set; }
    }
}
