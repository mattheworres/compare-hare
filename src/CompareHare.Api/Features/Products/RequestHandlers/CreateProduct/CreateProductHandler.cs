using AutoMapper;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products.RequestHandlers.CreateProduct
{
    public class CreateProductHandler : ApiRequestHandlerBase, IRequestHandler<CreateProductMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateProductHandler(
            CompareHareDbContext dbContext,
            IMapper mapper) {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(CreateProductMessage message, CancellationToken cancellationToken)
        {
            var model = message.Model;

            var newProduct = _mapper.Map<TrackedProduct>(model);

            await _dbContext.TrackedProducts.AddAsync(newProduct);
            await _dbContext.SaveChangesAsync();

            var newRetailers = new List<TrackedProductRetailer>();

            foreach(var retailer in model.ProductRetailers) {
                retailer.TrackedProductId = newProduct.Id;
                var newRetailer = _mapper.Map<TrackedProductRetailer>(retailer);
                newRetailers.Add(newRetailer);
            }

            await _dbContext.TrackedProductRetailers.AddRangeAsync(newRetailers);
            await _dbContext.SaveChangesAsync();

            // TODO: update frontend to take into consideration 0 scrapes with retailers
            return Ok(newProduct.Id);
        }
    }
}
