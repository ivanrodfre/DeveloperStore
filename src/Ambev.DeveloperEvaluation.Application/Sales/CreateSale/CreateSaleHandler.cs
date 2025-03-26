using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handles the creation of a new sale by processing the <see cref="CreateSaleCommand"/>.
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ISaleService _saleService;

        //private readonly IValidator<CreateSaleCommand> _validator;


        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSaleHandler"/> class.
        /// </summary>
        /// <param name="saleRepository">The sale repository used to persist the sale.</param>
        /// <param name="mapper">The mapper used to convert between domain entities and DTOs.</param>
        /// <param name="validator">The validator to validate the sale creation command.</param>
        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ISaleService saleService
            /*, IValidator<CreateSaleCommand> validator*/)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _saleService = saleService;
            //_validator = validator;
        }


        /// <summary>
        /// Handles the creation of a new sale.
        /// </summary>
        /// <param name="command">The command containing sale creation details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="CreateSaleResult"/>
        /// with the details of the created sale.
        /// </returns>
        /// <exception cref="ValidationException">
        /// Thrown when the sale command fails validation either in the command validator or in the domain business rules.
        /// </exception>
        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {

            var validator = new CreateSaleCommandValidator();
            var validationCommand = await validator.ValidateAsync(command, cancellationToken);
            if (!validationCommand.IsValid)
                throw new ValidationException(validationCommand.Errors);

            var products = command.Products.Select(produto =>
                Product.CreateProduct(produto.ProductId, command.Id, produto.Quantity, produto.UnitPrice)).ToList();


            var sale = Sale.CreateSale(
                command.SaleNumber,
                command.SaleDate,
                command.CustomerId,
                command.BranchId,
                products
                );


            //Todo
            //Implementar as regras de domínio mais genérica
            var discountValidation = sale.ApplyDiscountRules();
            if (!discountValidation.IsValid)
                throw new ValidationException(discountValidation.Errors);     
            
            await _saleRepository.CreateAsync(sale, cancellationToken);
            var result = _mapper.Map<CreateSaleResult>(sale);

            await _saleService.SendSaleEventAsync(sale.Id, "Sale created successfully!");

            return result;          

        }
    }
}
