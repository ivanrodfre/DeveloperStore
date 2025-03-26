namespace Ambev.DeveloperEvaluation.Domain.Events.Sales
{
    public class SaleCancelledEvent
    {
        public int SaleId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
