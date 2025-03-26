using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sales
{
    public class EventMessage : BaseEntity
    {
        public Guid SaleId { get; set; }
        public string EventMessageItem { get; set; }

        public EventMessage()
        {
            Id = Guid.NewGuid();
        }

        public EventMessage(Guid saleId, string eventMessage)
        {
            SaleId = saleId;
            EventMessageItem = eventMessage;
        }

    }
}
