namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;


/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// Gets or sets the sale number.
    /// Must be unique and contain only valid characters.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of the sale.
    /// </summary>
    public DateTime SaleDate { get; set; }


    /// <summary>
    /// Gets or sets the identifier of the customer associated with the sale.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the branch where the sale was made.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the collection of sale items.
    /// This list must contain at least one item.
    /// </summary>
    public required List<CreateProductRequest> Products { get; set; } = [];

}

/// <summary>
/// Represents an item included in a sale creation request.
/// </summary>
public class CreateProductRequest
{
    /// <summary>
    /// Gets or sets the product identifier.
    /// Must be a valid non-empty GUID.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product being sold.
    /// Must be a positive integer.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product in the sale.
    /// </summary>
    public decimal UnitPrice { get; set; }
}

