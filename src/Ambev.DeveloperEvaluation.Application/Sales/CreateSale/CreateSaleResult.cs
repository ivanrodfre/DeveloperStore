namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Represents the result of a sale creation operation, containing the sale details.
    /// </summary>
    public class CreateSaleResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the sale number.
        /// </summary>
        public string SaleNumber { get; set; }

        /// <summary>
        /// Gets or sets the date when the sale was made.
        /// </summary>
        public DateTime SaleDate { get; set; }


        /// <summary>
        /// Gets or sets the unique identifier of the customer associated with the sale.
        /// </summary>
        public Guid CustomerId { get; set; }


        /// <summary>
        /// Gets or sets the unique identifier of the branch where the sale was made.
        /// </summary>
        public Guid BranchId { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the sale has been cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        public List<CreateProductResult> Products { get; set; }
    }


    /// <summary>
    /// Represents the detailed result for an individual sale item.
    /// </summary>
    public class CreateProductResult
    {

        /// <summary>
        /// Gets or sets the unique identifier of the sale item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the sale to which this item belongs.
        /// </summary>
        public Guid SaleId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        public Guid ProductId { get; set; }


        /// <summary>
        /// Gets or sets the quantity of the product sold.
        /// </summary>
        public int Quantity { get; set; }


        /// <summary>
        /// Gets or sets the unit price of the product at the time of sale.
        /// </summary>
        public decimal UnitPrice { get; set; }


        /// <summary>
        /// Gets or sets the discount applied to the product.
        /// Expressed as a decimal (e.g., 0.10 for 10% discount).
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Gets or sets the total price for the sale item after applying the discount.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }

}
