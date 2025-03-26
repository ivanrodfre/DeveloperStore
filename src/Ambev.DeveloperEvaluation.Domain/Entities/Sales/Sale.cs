using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sales;
public class Sale : BaseEntity
{
    public string SaleNumber { get; private set; }
    public DateTime SaleDate { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid BranchId { get; private set; }
    public bool IsCancelled { get; private set; }
    public bool IsCheckoutCompleted { get; private set; }



    private readonly List<object> _domainEvents = new List<object>();
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        
    private readonly List<Product> _products = new List<Product>();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Sale() { }

    private Sale(Guid id, string saleNumber, DateTime saleDate, Guid customerId, Guid branchId)
    {
        Id = id;
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        BranchId = branchId;
        IsCancelled = false;
        IsCheckoutCompleted = false;
    }

    public void RemoveProduct(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        _products.Remove(product);
    }

    public void AddProduct(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        _products.Add(product);
    }

    public void CheckoutCompleted()
    {
        ValidationCheckout();
    }

    private void ValidationCheckout()
    {
        // Ensure the sale contains at least one item.
        if (_products == null || !_products.Any())
            throw new InvalidOperationException("The sale must contain at least one item to be finalized.");

        // Calculate the total amount of the sale.
        decimal totalAmount = _products.Sum(product => product.TotalPrice);
        if (totalAmount <= 0)
            throw new InvalidOperationException("The total amount of the sale must be greater than zero.");

        // The sale must not be cancelled.
        if (IsCancelled)
            throw new InvalidOperationException("Cannot finalize a cancelled sale.");

        // All validations passed: mark the sale Is CheckoutCompleted.
        IsCheckoutCompleted = true;
    }

    public void UpdateSale(
        string saleNumber,
        DateTime saleDate,
        Guid customerId,
        Guid branchId,
        List<Product> novosProducts)
    {
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        BranchId = branchId;

        _products.Clear();
        foreach (var product in novosProducts)
        {
            _products.Add(product);
        }


        var sale = this;
        sale.AddDomainEvent(new SaleModified
        {
            SaleId = sale.Id,
            CreatedAt = sale.SaleDate
        });

    }

    /// <summary>
    /// Create sale
    /// </summary>
    public static Sale CreateSale(
        string saleNumber,
        DateTime saleDate,
        Guid customerId,
        Guid branchId,
        List<Product> products)
    {
        if (string.IsNullOrWhiteSpace(saleNumber))
            throw new ArgumentException("The sale number is required.", nameof(saleNumber));

        if (products == null || !products.Any())
            throw new ArgumentException("The sale must contain at least one product.", nameof(products));

        var sale = new Sale(Guid.NewGuid(), saleNumber, saleDate, customerId, branchId);
        sale.AddProducts(products);


        sale.AddDomainEvent(new SaleCreatedEvent
        {
            SaleNumber = sale.SaleNumber,
            CreatedAt = sale.SaleDate
        });


        return sale;
    }

    /// <summary>
    /// Add items for sale.
    /// </summary>
    protected void AddProducts(IEnumerable<Product> products)
    {
        foreach (var product in products)
        {
            _products.Add(product);
        }
    }

    /// <summary>
    /// Applies discount rules to items and checks restrictions.
    /// - Items with quantities greater than 20 generate an error.
    /// - Items with 10 to 20 units receive a 20% discount.
    /// - Items with 4 to 9 units receive a 10% discount.
    /// - Items with less than 4 units do not receive a discount.
    /// </summary>
    public ValidationResult ApplyDiscountRules()
    {
        var erros = new List<ValidationFailure>();

        foreach (var product in _products)
        {
            if (product.Quantity > 20)
            {
                erros.Add(new ValidationFailure("Quantity", "It is not possible to sell more than 20 units of the same product."));
                continue;
            }

            if (product.Quantity >= 10 && product.Quantity <= 20)
                product.Discount = 0.20m;
            else if (product.Quantity >= 4 && product.Quantity < 10)
                product.Discount = 0.10m;
            else
                product.Discount = 0.0m;

            product.CalculateTotalPrice();
        }

        return new ValidationResult(erros);
    }

    /// <summary>
    /// Method to cancel the sale.
    /// </summary>
    public void Cancel()
    {
        if (!IsCancelled)
        {
            IsCancelled = true;

            var sale = this;
            sale.AddDomainEvent(new SaleCreatedEvent
            {
                SaleId = sale.Id,
                CreatedAt = sale.SaleDate
            });
        }
    }


    private void AddDomainEvent(object eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }


}


