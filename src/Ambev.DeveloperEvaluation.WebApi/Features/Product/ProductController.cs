using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Products;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.Validations;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;        

        public ProductController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // GET: api/Product
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(ApiResponseWithData<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll([FromQuery] QueryParametersCommand parameters, CancellationToken cancellationToken)
        {
            parameters.Filters = GetAllQueryParameters();

            var command = new GetProductQueryCommand { QueryParameters = parameters };
            var validator = new GetProductQueryValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<List<ProductDto>>(result.Products);

            var paginatedList = PaginatedList<ProductDto>.Create(
                response,
                result.TotalCount,
                parameters._page,
                parameters._size
            );

            return OkPaginated(paginatedList);
        }

        // GET: api/Product/{id}
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(ApiResponseWithData<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var command = new GetProductByIdCommand(id);
            var validator = new IdValidator();
            var validationResult = await validator.ValidateAsync(command.Id, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _mediator.Send(command, cancellationToken);

            if (result == null)
                return NotFound();

            var response = _mapper.Map<ProductDto>(result);
            return Ok(response);
        }

        // POST: api/Product
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<ProductDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateProductValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _mediator.Send(command, cancellationToken);

            var response = _mapper.Map<ProductDto>(result);
            return Ok(response);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(typeof(ApiResponseWithData<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var validator = new UpdateProductValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _mediator.Send(command, cancellationToken);

            if (result == null)
                return NotFound();

            var response = _mapper.Map<ProductDto>(result);
            return Ok(response);
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductByIdCommand(id);
            var validator = new IdValidator();
            var validationResult = await validator.ValidateAsync(command.Id, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _mediator.Send(command, cancellationToken);

            if (!result)
                return NotFound();

            return OkMessage("Product deleted successfully");
        }
    }
}
