using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public class SaleService : ISaleService
    {
        //private readonly IBus _bus;
        private readonly IEventRepository _eventRepository;

        public SaleService(
            //IBus bus, 
            IEventRepository eventRepository)
        {
            //_bus = bus;
            _eventRepository = eventRepository;
        }

        public async Task SendSaleEventAsync(Guid saleId, string customerMessage)
        {

            //Todo 
            // Publica os eventos de domínio
            //foreach (var domainEvent in sale.DomainEvents)
            //{
            //    await _bus.Publish(domainEvent);
            //}
            //sale.ClearDomainEvents();

            var evento = new EventMessage(saleId, customerMessage);

            await _eventRepository.CreateAsync(evento);


            ///await _bus.Publish();


        }
    }
}
