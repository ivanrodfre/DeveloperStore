using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Unit.Application.CreateSaleHandlerTests;
using AutoMapper;
using Bogus;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

//NSubstitute, Bogus e xUnit
public class CreateSaleHandlerAdditionalTests
{
    private readonly ISaleRepository _saleRepositoryMock;
    private readonly IMapper _mapperMock;
    private readonly ISaleService _saleServiceMock;
    private readonly CreateSaleHandler _handler;
    private readonly Faker _faker;

    public CreateSaleHandlerAdditionalTests()
    {
        _saleRepositoryMock = Substitute.For<ISaleRepository>();
        _mapperMock = Substitute.For<IMapper>();
        _saleServiceMock = Substitute.For<ISaleService>();

        _saleRepositoryMock
            .CreateAsync(Arg.Any<Sale>())
            .Returns(ci => Task.FromResult(ci.Arg<Sale>()));

        
        _mapperMock.Map<CreateSaleResult>(Arg.Any<Sale>())
            .Returns(ci =>
            {
                var sale = ci.Arg<Sale>();
                return new CreateSaleResult
                {
                    Id = sale.Id,
                    SaleNumber = sale.SaleNumber,
                    Products = sale.Products.Select(product => new CreateProductResult
                    {
                        ProductId = product.ProductId,
                        Quantity = product.Quantity,
                        UnitPrice = product.UnitPrice,
                        Discount = product.Discount,
                        TotalPrice = product.TotalPrice
                    }).ToList()
                };
            });


        _handler = new CreateSaleHandler(_saleRepositoryMock, _mapperMock, _saleServiceMock);
        _faker = new Faker("pt_BR");
    }

    //[Fact]
    //Invalidar teste, pois precisa ser um ValidationException
    //public async Task Handle_EmptySaleNumber_ShouldThrowArgumentException()
    //{
    //    // Arrange: gera um comando válido e, em seguida, define o número da venda como vazio
    //    var command = CreateSaleHandlerTestData.GenerateValidCommand();
    //    command.SaleNumber = "";  // Número de venda inválido

    //    // Act & Assert
    //    await Assert.ThrowsAsync<ValidationException>(() =>
    //        _handler.Handle(command, CancellationToken.None));

    //    // Garante que o repositório não foi chamado
    //    await _saleRepositoryMock.DidNotReceive().CreateAsync(Arg.Any<Sale>());
    //}

    //[Fact]
    //Invalidar teste, pois precisa ser um ValidationException
    //public async Task Handle_NullItems_ShouldThrowValidationException()
    //{
    //    // Arrange: gera um comando válido e define a lista de itens como null
    //    var command = CreateSaleHandlerTestData.GenerateValidCommand();
    //    command.Items = null;  // Itens nulos

    //    // Act & Assert
    //    await Assert.ThrowsAsync<ValidationException>(() =>
    //        _handler.Handle(command, CancellationToken.None));

    //    await _saleRepositoryMock.DidNotReceive().CreateAsync(Arg.Any<Sale>());
    //}

    [Fact]
    public async Task Handle_ItemWithZeroQuantity_ShouldThrowArgumentException()
    {
        // Arrange: gera um comando válido e altera a quantidade de um produto para zero
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        // Altera o primeiro item para ter quantidade zero (inválido)
        command.Products[0].Quantity = 0;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(command, CancellationToken.None));

        await _saleRepositoryMock.DidNotReceive().CreateAsync(Arg.Any<Sale>());
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateSale_WithExpectedDiscounts()
    {
        // Arrange: Gere um comando válido com 2 itens com quantidades específicas
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        command.Products = new List<CreateProductDto>
        {
            // Primeiro item: 5 unidades (deve receber 10% de desconto)
            new CreateProductDto { ProductId = Guid.NewGuid(), Quantity = 5, UnitPrice = 100 },
            // Segundo item: 12 unidades (deve receber 20% de desconto)
            new CreateProductDto { ProductId = Guid.NewGuid(), Quantity = 12, UnitPrice = 50 }
        };


        Sale capturedSale = null;
        _saleRepositoryMock.CreateAsync(Arg.Any<Sale>())
            .Returns(ci =>
            {
                capturedSale = ci.Arg<Sale>();
                return Task.FromResult(capturedSale);
            });

        // Act: Chama o handler
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Verifica o resultado mapeado e a instância capturada
        Assert.NotNull(result);
        Assert.Equal(command.SaleNumber, result.SaleNumber);
        Assert.NotEqual(default(Guid), result.Id);

        // Verifica se capturamos a venda criada e se os itens foram populados
        Assert.NotNull(capturedSale);
        Assert.NotNull(capturedSale.Products);
        var products = capturedSale.Products.ToList();
        Assert.Equal(2, products.Count);

        // Primeiro item: 5 unidades => 10% de desconto
        Assert.Equal(0.10m, products[0].Discount);
        Assert.Equal(5 * 100 * (1 - 0.10m), products[0].TotalPrice);

        // Segundo item: 12 unidades => 20% de desconto
        Assert.Equal(0.20m, products[1].Discount);
        Assert.Equal(12 * 50 * (1 - 0.20m), products[1].TotalPrice);
    }

}
