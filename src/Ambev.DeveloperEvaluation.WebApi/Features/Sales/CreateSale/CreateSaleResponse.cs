namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{

    /// <summary>
    /// Represents the response data for a created sale.
    /// </summary>
    /// <param name="Id">The unique identifier of the sale.</param>
    /// <param name="SaleNumber">The sale number.</param>
    /// <param name="SaleDate">The date when the sale was made.</param>
    /// <param name="CustomerId">The identifier of the customer associated with the sale.</param>
    /// <param name="BranchId">The identifier of the branch where the sale was made.</param>
    /// <param name="IsCancelled">Indicates whether the sale has been cancelled.</param>
    /// <param name="Items">A collection of sale item responses associated with the sale.</param>
    public record CreateSaleResponse(
        Guid Id,
        string SaleNumber,
        DateTime SaleDate,
        Guid CustomerId,
        Guid BranchId,
        bool IsCancelled,
        IReadOnlyCollection<CreateProductResponse> Products
    );


    /// <summary>
    /// Represents the response data for an individual sale item.
    /// </summary>
    /// <param name="Id">The unique identifier of the sale item.</param>
    /// <param name="SaleId">The identifier of the sale to which this item belongs.</param>
    /// <param name="ProductId">The identifier of the product.</param>
    /// <param name="Quantity">The quantity of the product sold.</param>
    /// <param name="UnitPrice">The unit price of the product at the time of sale.</param>
    /// <param name="Discount">The discount applied to the product, expressed as a decimal (e.g., 0.10 for 10%).</param>
    /// <param name="TotalPrice">The total price for this sale item after applying the discount.</param>
    public record CreateProductResponse(
        Guid Id,
        Guid SaleId,
        Guid ProductId,
        int Quantity,
        decimal UnitPrice,
        decimal Discount,
        decimal TotalPrice
    );
}

