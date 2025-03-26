using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{

    /// <summary>
    /// Validates the properties of the <see cref="CreateSaleCommand"/>.
    /// </summary>
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSaleCommandValidator"/> class.
        /// Defines the validation rules for creating a sale.
        /// </summary>
        public CreateSaleCommandValidator()
        {
            RuleFor(x => x.SaleNumber)
                .NotEmpty()
                .WithMessage("Sale number is required.");

            RuleFor(x => x.SaleDate)
                .NotEmpty()
                .WithMessage("Sale date is required.");

            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("A valid customer ID is required.");

            RuleFor(x => x.Products)
                .NotNull()
                .WithMessage("Sale cannot be completed without selected products.");
        }
    }

}
