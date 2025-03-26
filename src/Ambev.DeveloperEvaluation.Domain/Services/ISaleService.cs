namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface ISaleService
    {
        Task SendSaleEventAsync(Guid saleId, string customerMessage);
    }
}
