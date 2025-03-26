namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public record UpdateSaleResponse(
        Guid? id,
        string SaleNumber,
        DateTime SaleDate,
        Guid CustomerId,
        Guid BranchId,
        bool IsCancelled,
        IReadOnlyCollection<UpdateProductResponse> Products
    );

    public record UpdateProductResponse(
        Guid ProductId,
        int Quantity,
        decimal UnitPrice,
        decimal Discount
    );
}
