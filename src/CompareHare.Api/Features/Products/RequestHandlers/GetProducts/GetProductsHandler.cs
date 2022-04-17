using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Products.RequestHandlers.GetProducts
{
    public class GetProductsHandler : ApiRequestHandlerBase, IRequestHandler<GetProductsMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IMapper _mapper;

        public GetProductsHandler(CompareHareDbContext dbContext, ICurrentUserProvider currentUserProvider, IMapper mapper)
        {
            _dbContext = dbContext;
            _currentUserProvider = currentUserProvider;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetProductsMessage request, CancellationToken cancellationToken)
        {
            var currentUserId = await _currentUserProvider.GetUserIdAsync();
            var userProducts = await _dbContext.TrackedProducts
                .Where(x => x.UserId == currentUserId)
                .Include(x => x.Retailers)
                .ThenInclude(x => x.ProductRetailerPrices)
                .ToListAsync();

            var models = _mapper.Map<IEnumerable<ProductListModel>>(userProducts);

            foreach(var productModel in models) {
                var matchingProduct = userProducts.First(x => x.Id == productModel.Id);

                if (matchingProduct.Prices.Any()) {
                    var lowestPrice = matchingProduct.Prices.OrderBy(x => x.Price).First();
                    // This may not work. spidey sense tingling?
                    _mapper.Map(lowestPrice, productModel);
                }
            }

            return Ok(models);
        }
    }
}
