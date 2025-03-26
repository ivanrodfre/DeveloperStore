using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.CreateSaleHandlerTests
{
    public static class CreateSaleHandlerTestData
    {
        // Gera um comando válido de criação de venda
        public static CreateSaleCommand GenerateValidCommand()
        {
            var faker = new Faker("pt_BR");

            // Configura itens
            var productFaker = new Faker<CreateProductDto>("pt_BR")
                .RuleFor(i => i.ProductId, f => Guid.NewGuid())
                .RuleFor(i => i.Quantity, f => f.Random.Int(4, 10)) // 4 a 10 para testar desconto de 10% e 20%
                .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(10, 200));

            return new CreateSaleCommand
            {
                SaleNumber = faker.Commerce.Ean13(),
                SaleDate = faker.Date.Recent(),
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Products = productFaker.Generate(faker.Random.Int(1, 3)) // Gera 1 a 3 itens
            };
        }

        // Gera um comando com item inválido (ex.: quantidade > 20)
        public static CreateSaleCommand GenerateInvalidItemCommand()
        {
            var faker = new Faker("pt_BR");

            var productFaker = new Faker<CreateProductDto>("pt_BR")
                .RuleFor(i => i.ProductId, f => Guid.NewGuid())
                .RuleFor(i => i.Quantity, 25) // Inválido, pois > 20
                .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(10, 200));

            return new CreateSaleCommand
            {
                SaleNumber = faker.Commerce.Ean13(),
                SaleDate = faker.Date.Recent(),
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Products = productFaker.Generate(1)
            };
        }
    }
}
