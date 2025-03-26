using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {

            RuleFor(x => x.SaleNumber).NotEmpty().NotNull()
                .WithMessage("Número do pedido inválido!");

            RuleFor(x => x.CustomerId).NotEmpty().NotNull()
                .WithMessage("Cliente é obrigatório!");

        }
    }
}
