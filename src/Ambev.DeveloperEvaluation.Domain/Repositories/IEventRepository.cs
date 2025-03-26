using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for User entity operations
/// </summary>
public interface IEventRepository
{
    /// <summary>
    /// Creates a new sale in the repository
    /// </summary>
    /// <param name="eventMessage">The event to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created event</returns>
    Task<Event> CreateAsync(Event eventMessage, CancellationToken cancellationToken = default);
   
}
