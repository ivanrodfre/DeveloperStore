using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            CreateMap<CreateProductRequest, CreateProductDto>();
            CreateMap<CreateProductResult, CreateProductResponse>();
            CreateMap<CreateSaleResult, CreateSaleResponse>();
            CreateMap<CreateSaleRequest, CreateSaleCommand>();           
        }
    }
}
