using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sales;
public class Product : BaseEntity
{
    public Guid ProductId { get; private set; }
    public Guid SaleId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; set; } 
    public decimal TotalPrice { get; private set; }


    private Product(Guid id, Guid productId, Guid saleId, int quantity, decimal unitPrice)
    {
        Id = id;
        ProductId = productId;
        SaleId = saleId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void UpdateProduct(int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        if (unitPrice <= 0)
            throw new ArgumentException("Unit price must be greater than zero.", nameof(unitPrice));

        Quantity = quantity;
        UnitPrice = unitPrice;

        CalculateTotalPrice();
    }

    /// <summary>
    /// Creates a new sales item, applying basic validations.
    /// </summary>
    public static Product CreateProduct(Guid productId, Guid saleId, int quantity, decimal unitPrice)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("ProductId invalid.", nameof(productId));
        if (quantity <= 0)
            throw new ArgumentException("The quantity must be greater than zero.", nameof(quantity));
        if (unitPrice <= 0)
            throw new ArgumentException("The unit price must be greater than zero.", nameof(unitPrice));

        return new Product(Guid.NewGuid(), productId, saleId,  quantity, unitPrice);
    }

    /// <summary>
    /// Calculates the item total considering the discount.
    /// </summary>
    public void CalculateTotalPrice()
    {
        TotalPrice = Quantity * UnitPrice * (1 - Discount);
    }
}
