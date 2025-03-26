using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CheckoutSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CheckoutSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    /// <summary>
    /// Controller for managing sales operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ISaleRepository _saleRepository;


        /// <summary>
        /// Initializes a new instance of SalesController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="saleRepository">The SaleRepository instance</param>
        public SalesController(IMediator mediator, IMapper mapper, ISaleRepository saleRepository)
        {
            _mediator = mediator;
            _mapper = mapper;
            _saleRepository = saleRepository;
        }

        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="request">The sale creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {

            var validator = new CreateSaleRequestValidator();
            var validationRequest = await validator.ValidateAsync(request, cancellationToken);

            if (!validationRequest.IsValid)
                return BadRequest(validationRequest.Errors);

            var command = _mapper.Map<CreateSaleCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Sale created successfully",
                Data = _mapper.Map<CreateSaleResponse>(response)
            });

        }


        /// <summary>
        /// Updates an existing sale with the provided details.
        /// </summary>
        /// <param name="id">The unique identifier of the sale to update.</param>
        /// <param name="request">The update sale request data.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The sale details if found</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSale(Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
        {

            if (id != request.Id)
                return BadRequest("Divergent IDs.");

            var validator = new UpdateSaleRequestValidator();
            var validationRequest = await validator.ValidateAsync(request, cancellationToken);

            if (!validationRequest.IsValid)
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Validation errors occurred.",
                    //Errors = validationRequest.Errors.Select(e => e.ErrorMessage)
                });

            var command = _mapper.Map<UpdateSaleCommand>(request);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<UpdateSaleResponse>
            {
                Success = true,
                Message = "Sale updated successfully.",
                Data = _mapper.Map<UpdateSaleResponse>(response)
            });

        }



        /// <summary>
        /// Checkout a sale (checkout process) based on its unique identifier.
        /// </summary>
        ///  /// <param name="SaleId">The unique identifier of the sale to finalize.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        [HttpPost("checkout/{SaleId}")]
        [ProducesResponseType(typeof(ApiResponseWithData<CheckoutSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Checkout([FromRoute] Guid SaleId, CancellationToken cancellationToken)
        {
            var command = new CheckoutSaleCommand { SaleId = SaleId };

            var response = await _mediator.Send(command, cancellationToken);
            
            return Ok(new ApiResponseWithData<CheckoutSaleResponse>
            {
                Success = true,
                Message = "Checkout completed successfully.",
                Data = _mapper.Map<CheckoutSaleResponse>(response)
            });
        }


        /// <summary>
        /// Checkout a sale (checkout process) based on its unique identifier.
        /// </summary>
        ///  /// <param name="SaleId">The unique identifier of the sale to finalize.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<CheckoutSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelSale(Guid id)
        {
            var response = await _mediator.Send(new CancelSaleCommand { Id = id });

            return Ok(new ApiResponseWithData<CheckoutSaleResponse>
            {
                Success = true,
                Message = "Sale Cancelede successfully.",
                Data = _mapper.Map<CheckoutSaleResponse>(response)
            });
        }


        /// <summary>
        /// Retrieves a sale by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale details if found</returns>

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSaleById([FromRoute] Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);

            if (sale == null) return NotFound();
            return Ok(sale);
        }


        /// <summary>
        /// Retrieves all sales
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale details if found</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllSale()
            => Ok(await _saleRepository.GetAllAsync());

    }

}
