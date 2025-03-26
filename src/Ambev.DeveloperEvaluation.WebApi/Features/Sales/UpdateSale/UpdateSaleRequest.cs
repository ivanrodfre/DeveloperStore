namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{

    /// <summary>
    /// Represents a request to update an existing sale.
    /// </summary>
    public class UpdateSaleRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the sale number.
        /// </summary>
        public string? SaleNumber { get; set; }

        /// <summary>
        /// Gets or sets the date when the sale occurred.
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
        /// Gets or sets the collection of sale item requests associated with the sale.
        /// </summary>
        public List<UpdateProductRequest> Products { get; set; } = new List<UpdateProductRequest>();

    }


    /// <summary>
    /// Represents a request to update an individual sale item.
    /// </summary>
    public class UpdateProductRequest
    {

        /// <summary>
        /// Gets or sets the unique identifier of the sale item.
        /// </summary>
        public Guid Id { get; set; }


        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        public Guid ProductId { get; set; }


        /// <summary>
        /// Gets or sets the quantity of the product being sold.
        /// </summary>
        public int Quantity { get; set; }


        /// <summary>
        /// Gets or sets the unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
