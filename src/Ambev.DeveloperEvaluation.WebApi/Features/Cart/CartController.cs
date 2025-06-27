using Ambev.DeveloperEvaluation.Application.Carts;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Application.Carts.Validations;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CartController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var command = new GetCartByIdCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return NotFound();

            var dto = _mapper.Map<CartDto>(result);
            return Ok(dto);
        }

        [HttpGet("GetByUserId/{id:Guid}")]
        public async Task<IActionResult> GetByUserId(Guid id, CancellationToken cancellationToken)
        {
            var command = new GetCartByUserIdCommand { UserId = id };
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return NotFound();

            var dto = _mapper.Map<CartDto>(result);
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CartDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCartCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateCartValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);


            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<CartDto>(result);

            return OkWithResponse(response);
        }

        [HttpPut("{id:Guid}")]
        [ProducesResponseType(typeof(ApiResponseWithData<CartDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCartCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return NotFound();

            return OkWithResponse(_mapper.Map<CartDto>(result));
        }

        [HttpDelete("DeleteCartProduct/{id:Guid}")]
        [ProducesResponseType(typeof(ApiResponseWithData<CartDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCartProduct(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteCartProductCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            if (!result)
                return NotFound();

            return OkMessage("Product removed from cart successfully");
        }

        [HttpDelete("DeleteAllCart/{id:Guid}")]
        [ProducesResponseType(typeof(ApiResponseWithData<CartDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAllCart(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteCartCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            if (!result)
                return NotFound();

            return OkMessage("Cart deleted successfully");
        }
    }
}