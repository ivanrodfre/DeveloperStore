using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleProfile : Profile
    {
        public UpdateSaleProfile()
        {
            CreateMap<UpdateSaleResult, UpdateSaleResponse>();
            CreateMap<ProductResult, UpdateProductResponse>();
            CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
            CreateMap<UpdateProductDto, UpdateProductRequest>();
            CreateMap<UpdateProductRequest, UpdateProductDto>();
            CreateMap<UpdateProductDto, ProductResult>();
            CreateMap<ProductResult, UpdateProductDto>();
        }
    }
}
