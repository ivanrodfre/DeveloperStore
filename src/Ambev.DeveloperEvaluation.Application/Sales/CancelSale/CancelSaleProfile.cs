using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleProfile : Profile
    {
        public CancelSaleProfile()
        {
            CreateMap<CancelSaleCommand, CancelSaleResult>();
            CreateMap<bool, CancelSaleResult>();
        }
    }
}
