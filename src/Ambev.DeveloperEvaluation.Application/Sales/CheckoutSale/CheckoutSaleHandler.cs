using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CheckoutSale
{

    public class CheckoutSaleHandler : IRequestHandler<CheckoutSaleCommand, CheckoutSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public CheckoutSaleHandler(
            ISaleRepository saleRepository,
            IMapper mapper
            )
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<CheckoutSaleResult> Handle(CheckoutSaleCommand command, CancellationToken cancellationToken)
        {

            var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
            if (sale == null)
                throw new ValidationException("Sale not found.");


            sale.CheckoutCompleted();

            await _saleRepository.UpdateAsync(sale, cancellationToken);

            var result = _mapper.Map<CheckoutSaleResult>(sale);
            return result;
        }
    }
}
