using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Products.RequestHandlers.GetProductCurrent
{
    public class GetProductCurrentHandler : ApiRequestHandlerBase, IRequestHandler<GetProductCurrentMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetProductCurrentHandler(CompareHareDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Handle(GetProductCurrentMessage message, CancellationToken cancellationToken)
        {
            var product = await _dbContext.TrackedProducts
                .Where(x => x.Id == message.Model.TrackedProductId)
                .Include(x => x.Prices)
                .Include(x => x.Retailers)
                .FirstAsync();

            var model = _mapper.Map<ProductCurrentDisplayModel>(product);
            model.ProductRetailers = _mapper.Map<IEnumerable<ProductRetailersListModel>>(product.Prices);

            return Ok(model);
        }
    }
}