using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using AutoMapper;


namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile() {
            CreateMap<CreateSaleCommand, CreateSaleResult>();
            CreateMap<Sale, CreateSaleResult>();
            CreateMap<Product, CreateProductResult>();
        }
    }
}
