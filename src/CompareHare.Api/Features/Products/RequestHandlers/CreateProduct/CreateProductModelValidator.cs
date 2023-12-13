using System;
using CompareHare.Api.Features.Products.Models;
using CompareHare.Domain.Entities.Constants;
using FluentValidation;

namespace CompareHare.Api.Features.Products.RequestHandlers.CreateProduct
{
    public class CreateProductModelValidator : AbstractValidator<CreateProductModel>
    {
        public CreateProductModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(1, 512).WithMessage("Name can only be up to 512 characters");

            RuleFor(x => x.ProductRetailers)
                .SetCollectionValidator(new CreateProductRetailerModelValidator());
        }
    }

    public class CreateProductRetailerModelValidator : AbstractValidator<CreateProductRetailerModel>
    {
        public CreateProductRetailerModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.TrackedProductId)
                .Null();

            RuleFor(x => x.ProductRetailer)
                .Must(retailer => Enum.IsDefined(typeof(ProductRetailer), retailer))
                .WithMessage("Incorrect retailer value");

            RuleFor(x => x.PriceSelector)
                .NotEmpty()
                .When(x => x.ProductRetailer == (int)ProductRetailer.Other)
                .WithMessage("Price selector is required for other retailers");

            RuleFor(x => x.PriceSelector)
                .Length(1,128)
                .When(x => x.ProductRetailer == (int)ProductRetailer.Other)
                .WithMessage("Price selector must be up to 128 characters");

            RuleFor(x => x.OtherRetailerDisplayName)
                .NotEmpty()
                .When(x => x.ProductRetailer == (int)ProductRetailer.Other)
                .WithMessage("Retailer name is required for other retailers");

            RuleFor(x => x.ScrapeUrl)
                .NotEmpty().WithMessage("Scrape URL is required")
                .Length(4, 512).WithMessage("Scrape URL must be up to 512 characters");
        }
    }
}
