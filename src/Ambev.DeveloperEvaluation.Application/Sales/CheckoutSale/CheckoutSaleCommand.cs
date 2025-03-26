using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CheckoutSale
{
    public class CheckoutSaleCommand : IRequest<CheckoutSaleResult>
    {
        public Guid SaleId { get; set; }
    }
}
