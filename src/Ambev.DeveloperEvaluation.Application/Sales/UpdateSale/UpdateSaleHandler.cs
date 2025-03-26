using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{

    /// <summary>
    /// Handles the update of an existing sale by processing the <see cref="UpdateSaleCommand"/>.
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        //private readonly IValidator<UpdateSaleCommand> _validator;


        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSaleHandler"/> class.
        /// </summary>
        /// <param name="saleRepository">The repository used to persist and retrieve sale data.</param>
        /// <param name="mapper">The mapper used for converting between domain entities and DTOs.</param>
        /// <param name="validator">The validator used to validate the update sale command.</param>
        public UpdateSaleHandler(ISaleRepository saleRepository, 
            IMapper mapper
            /*IValidator<UpdateSaleCommand> validator*/)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            //_validator = validator;
        }



        /// <summary>
        /// Handles the update sale command.
        /// </summary>
        /// <param name="command">The command containing the updated sale data.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an <see cref="UpdateSaleResult"/>
        /// with the updated sale details.
        /// </returns>
        /// <exception cref="ValidationException">Thrown when the update sale command fails validation.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the sale to update does not exist.</exception>
        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            //Todo
            //Criar ValidatorUpdate

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (sale == null)
                throw new ValidationException("Sale not found.");

            // and that command.Id contains the ID of the current sale.
            var updatedProducts = command.Products.Select(product =>
                Product.CreateProduct(product.ProductId, command.Id, product.Quantity, product.UnitPrice)
            ).ToList();

            SetSaleAndProducts(sale, updatedProducts);

            // Update basic sale properties.
            sale.UpdateSale(command.SaleNumber, command.SaleDate, command.CustomerId, command.BranchId, sale.Products.ToList());

            // Apply discount rules defined in the domain.
            var discountValidation = sale.ApplyDiscountRules();
            if (!discountValidation.IsValid)
                throw new ValidationException(discountValidation.Errors);

            await _saleRepository.UpdateRangeAsync(sale, cancellationToken);

            var result = _mapper.Map<UpdateSaleResult>(sale);
            return result;

        }

        private static void SetSaleAndProducts(Sale sale, List<Product> updatedProducts)
        {
            var updatedProductIds = updatedProducts.Where(i => i.Id != Guid.Empty).Select(i => i.Id).ToList();
            var productsToRemove = sale.Products.Where(i => i.Id != Guid.Empty && !updatedProductIds.Contains(i.Id)).ToList();
            foreach (var product in productsToRemove)
            {
                sale.RemoveProduct(product); // Método de domínio para remover o item.
            }

            // 2. Update existing items and add new items.
            foreach (var product in updatedProducts)
            {
                var existingProduct = sale.Products.FirstOrDefault(i => i.Id == product.Id);
                if (existingProduct != null)
                {
                    // Update the existing item using a domain method.
                    existingProduct.UpdateProduct(product.Quantity, product.UnitPrice);
                }
                else
                {
                    // Add new item if the item does not exist in the current sale.
                    sale.AddProduct(product);
                }
            }
        }
    }

}
