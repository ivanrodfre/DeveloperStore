using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly DefaultContext _context;

        public EventRepository(DefaultContext context)
        {
            _context = context;
        }

        

        public async Task<Event> CreateAsync(Event eventMessage, CancellationToken cancellationToken = default)
        {
            await _context.Events.AddAsync(eventMessage, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return eventMessage;
        }



    }

}
