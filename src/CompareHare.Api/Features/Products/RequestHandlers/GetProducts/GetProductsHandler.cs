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
                .Include(x => x.Prices)
                .ToListAsync();

            var models = _mapper.Map<IEnumerable<ProductListModel>>(userProducts);

            foreach(var productModel in models) {
                var matchingProduct = userProducts.First(x => x.Id == productModel.Id);

                if (matchingProduct != null && matchingProduct.Prices.Count() > 0) {
                    var lowestPrice = matchingProduct.Prices.OrderBy(x => x.Price).First();
                    _mapper.Map(lowestPrice, productModel);

                    productModel.HasScrapingExceptions = await _dbContext.ProductPriceScrapingExceptions.AnyAsync(x => x.TrackedProductId == productModel.Id);
                }
            }

            return Ok(models);
        }
    }
}
