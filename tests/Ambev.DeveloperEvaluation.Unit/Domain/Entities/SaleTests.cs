using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        [Fact]
        public void CriarVenda_WithValidData_ShouldCreateSaleWithItems()
        {
            // Arrange
            var saleId = new Guid(Guid.NewGuid().ToString());
            var saleNumber = "SALE-001";
            var saleDate = DateTime.UtcNow;
            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();
            var products = new List<Product>
            {
                Product.CreateProduct(Guid.NewGuid(),saleId, 5, 100),  // quantidade válida
                Product.CreateProduct(Guid.NewGuid(),saleId, 8, 150)
            };

            // Act
            var sale = Sale.CreateSale(saleNumber, saleDate, customerId, branchId, products);

            // Assert
            Assert.NotNull(sale);
            Assert.Equal(saleNumber, sale.SaleNumber);
            Assert.Equal(saleDate, sale.SaleDate);
            Assert.Equal(customerId, sale.CustomerId);
            Assert.Equal(branchId, sale.BranchId);
            Assert.NotNull(sale.Products);
            Assert.Equal(products.Count, sale.Products.Count);
        }

        [Fact]
        public void CriarVenda_WithEmptySaleNumber_ShouldThrowArgumentException()
        {
            // Arrange
            var saleId = new Guid(Guid.NewGuid().ToString());
            var saleDate = DateTime.UtcNow;
            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();
            var products = new List<Product>
            {
                Product.CreateProduct(Guid.NewGuid(),saleId, 5, 100)
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                Sale.CreateSale("", saleDate, customerId, branchId, products));
            Assert.Contains("The sale number is required.", exception.Message);
        }

        [Fact]
        public void CriarVenda_WithNullItems_ShouldThrowArgumentException()
        {
            // Arrange
            var saleNumber = "";
            var saleDate = DateTime.UtcNow;
            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();

            var products = new List<Product>
            {
                Product.CreateProduct(Guid.NewGuid(), branchId, 5, 100)
            };


            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                Sale.CreateSale(saleNumber, saleDate, customerId, branchId, products));
            Assert.Contains("The sale number is required.", exception.Message);
        }

        [Fact]
        public void AplicarRegrasDeDesconto_WithValidQuantities_ShouldApplyCorrectDiscounts()
        {

            var saleId = new Guid(Guid.NewGuid().ToString());

            // Arrange
            var product1 = Product.CreateProduct(Guid.NewGuid(), saleId, 5, 100);
            var product2 = Product.CreateProduct(Guid.NewGuid(), saleId, 12, 50);
            var sale = Sale.CreateSale("SALE-002", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid(), new List<Product> { product1, product2 });

            // Act
            var validationResult = sale.ApplyDiscountRules();

            // Assert
            Assert.True(validationResult.IsValid, "A validação deve ser válida para itens com quantidades dentro do permitido.");
            Assert.Equal(0.10m, product1.Discount);
            Assert.Equal(5 * 100 * (1 - 0.10m), product1.TotalPrice);
            Assert.Equal(0.20m, product2.Discount);
            Assert.Equal(12 * 50 * (1 - 0.20m), product2.TotalPrice);
        }

        [Fact]
        public void AplicarRegrasDeDesconto_WithQuantityAbove20_ShouldReturnValidationError()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            // Cria um produto com quantidade inválida (> 20)
            var product = Product.CreateProduct(Guid.NewGuid(), saleId, 21, 100);
            var sale = Sale.CreateSale("SALE-003", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid(), new List<Product> { product });

            // Act
            var validationResult = sale.ApplyDiscountRules();

            // Assert
            Assert.False(validationResult.IsValid, "A validação deveria falhar, pois a quantidade do produto é maior que 20.");
            Assert.NotNull(validationResult.Errors);
        }

        [Fact]
        public void AtualizarVenda_WithNewData_ShouldUpdatePropertiesAndItems()
        {

            var saleId = new Guid(Guid.NewGuid().ToString());

            // Arrange
            var originalProducts = new List<Product>
            {
                Product.CreateProduct(Guid.NewGuid(), saleId, 5, 100)
            };
            var sale = Sale.CreateSale("SALE-004", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid(), originalProducts);

            var newSaleNumber = "SALE-005";
            var newSaleDate = DateTime.UtcNow.AddDays(1);
            var newCustomerId = Guid.NewGuid();
            var newBranchId = Guid.NewGuid();
            var newProducts = new List<Product>
            {
                Product.CreateProduct(Guid.NewGuid(),saleId, 8, 200),
                Product.CreateProduct(Guid.NewGuid(), saleId, 4, 150)
            };

            // Act
            sale.UpdateSale(newSaleNumber, newSaleDate, newCustomerId, newBranchId, newProducts);

            // Assert
            Assert.Equal(newSaleNumber, sale.SaleNumber);
            Assert.Equal(newSaleDate, sale.SaleDate);
            Assert.Equal(newCustomerId, sale.CustomerId);
            Assert.Equal(newBranchId, sale.BranchId);
            Assert.Equal(newProducts.Count, sale.Products.Count);
        }

        [Fact]
        public void Cancel_WithNotCancelledSale_ShouldSetIsCancelledTrue()
        {
            var saleId = new Guid(Guid.NewGuid().ToString());


            // Arrange
            var sale = Sale.CreateSale("SALE-006", DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid(), new List<Product>
            {
                Product.CreateProduct(Guid.NewGuid(), saleId, 5, 100)
            });
            Assert.False(sale.IsCancelled, "A venda deve iniciar não cancelada.");

            // Act
            sale.Cancel();

            // Assert
            Assert.True(sale.IsCancelled, "Após cancelar, a venda deve estar marcada como cancelada.");
        }

    }
}
