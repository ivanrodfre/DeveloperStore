namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CheckoutSale
{
    public record CheckoutSaleResponse(
        Guid SaleId,
        bool IsCheckoutCompleted
    );

}
