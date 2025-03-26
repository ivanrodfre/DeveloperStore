using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.CreateSaleHandlerTests
{
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepositoryMock;
        private readonly ISaleService _saleServiceMock;
        private readonly IMapper _mapperMock;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            // Substitutos (mocks) criados com NSubstitute
            _saleRepositoryMock = Substitute.For<ISaleRepository>();
            _mapperMock = Substitute.For<IMapper>();
            _saleServiceMock = Substitute.For<ISaleService>();

            _saleRepositoryMock.CreateAsync(Arg.Any<Sale>())
                                .Returns(ci => Task.FromResult(ci.Arg<Sale>()));


            _mapperMock.Map<CreateSaleResult>(Arg.Any<Sale>())
                       .Returns(ci =>
                       {
                           var sale = ci.Arg<Sale>();
                           return new CreateSaleResult
                           {
                               Id = sale.Id,
                               SaleNumber = sale.SaleNumber
                           };
                       });

            _handler = new CreateSaleHandler(_saleRepositoryMock, _mapperMock, _saleServiceMock);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateSale()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidCommand();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(default, result.Id);
            Assert.Equal(command.SaleNumber, result.SaleNumber);

            // Verifica se o repositório foi chamado
            await _saleRepositoryMock.Received(1).CreateAsync(Arg.Any<Sale>());

            _mapperMock.Received(1).Map<CreateSaleResult>(Arg.Any<Sale>());
        }

        [Fact]
        public async Task Handle_InvalidItemQuantity_ShouldThrowValidationException()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateInvalidItemCommand();

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            await _saleRepositoryMock.DidNotReceive().CreateAsync(Arg.Any<Sale>());
        }
    }
}
