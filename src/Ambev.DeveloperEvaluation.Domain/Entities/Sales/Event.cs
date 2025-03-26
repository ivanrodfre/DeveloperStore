using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sales
{
    public class Event : BaseEntity
    {
        public Guid SaleId { get; set; }
        public string EventMessage { get; set; }


        public Event(Guid saleId, string eventMessage)
        {
            SaleId = saleId;
            EventMessage = eventMessage;
        }

    }
}
