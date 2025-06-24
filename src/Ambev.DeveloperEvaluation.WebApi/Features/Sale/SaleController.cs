using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSaleById;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.Validations;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SaleController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SaleController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var command = new GetSaleByIdCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return NotFound();

            var dto = _mapper.Map<SaleDto>(result);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryParametersCommand parameters, CancellationToken cancellationToken = default)
        {
            var command = new GetSalesQueryCommand { QueryParameters = parameters };
            var validator = new GetSaleQueryValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<List<SaleDto>>(result.Sales);

            var paginatedList = PaginatedList<SaleDto>.Create(
                response,
                result.TotalCount,
                parameters._page,
                parameters._size
            );

            return OkPaginated(paginatedList);
        }

        [HttpGet("GetSalePreviewByCart/{id:Guid}")]
        [ProducesResponseType(typeof(ApiResponseWithData<SaleDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSalePreviewByCart(Guid id, CancellationToken cancellationToken)
        {
            var command = new GetSalePreviewCommand(id);
            var validator = new CreateSalePreviewValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<SaleDto>(result);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<SaleDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<SaleDto>(result);

            return Ok(response);
        }

        [HttpPut("{id:Guid}")]
        [ProducesResponseType(typeof(ApiResponseWithData<SaleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<SaleDto>(result));
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(typeof(ApiResponseWithData<SaleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteSaleCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            if (!result)
                return NotFound();

            return OkMessage("Sale deleted successfully");
        }
    }
}