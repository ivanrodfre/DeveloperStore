using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{

    /// <summary>
    /// Validates the properties of the <see cref="CreateSaleRequest"/>.
    /// </summary>
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSaleRequestValidator"/> class.
        /// Defines the validation rules for creating a sale.
        /// </summary>
        public CreateSaleRequestValidator()
        {

            RuleFor(x => x.SaleNumber)
                .NotEmpty()
                .WithMessage("Sale number is required.");

            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("A customer is required to make a sale.");

            RuleFor(x => x.BranchId)
                .NotEmpty()
                .WithMessage("A branch is required to make a sale.");


            RuleFor(x => x.Products)
               .NotEmpty()
               .WithMessage("The list of Products cannot be empty.");

            RuleForEach(x => x.Products).ChildRules(products =>
            {
                products.RuleFor(i => i.ProductId)
                    .NotEmpty()
                    .WithMessage("Product ID is required.");
                products.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero.");
                products.RuleFor(i => i.UnitPrice)
                    .GreaterThan(0)
                    .WithMessage("Unit price must be greater than zero.");
            });

        }
    }
}
