﻿using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleCommand : IRequest<CancelSaleResult>
    {
        public Guid Id { get; set; }
        public bool Success { get; set; }
    }
}
