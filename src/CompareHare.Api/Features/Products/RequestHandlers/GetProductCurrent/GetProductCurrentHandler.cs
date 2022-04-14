using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
                .Include(x => x.Retailers)
                .Include(x => x.Prices)
                .FirstAsync();

            var model = _mapper.Map<ProductCurrentDisplayModel>(product);

            // Go by retailer to ensure added retailers are shown in list without prices
            foreach(var retailer in product.Retailers) {
                model.ProductRetailers.Add(
                        product.Prices.Any(x => x.TrackedProductRetailerId == retailer.Id)
                        ? _mapper.Map<ProductRetailersListModel>(product.Prices.FirstOrDefault(x => x.TrackedProductRetailerId == retailer.Id))
                        : _mapper.Map<ProductRetailersListModel>(retailer)
                    );
            }

            return Ok(model);
        }
    }
}