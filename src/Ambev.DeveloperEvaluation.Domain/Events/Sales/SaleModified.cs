namespace Ambev.DeveloperEvaluation.Domain.Events.Sales
{
    public class SaleModified
    {
        public Guid SaleId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
