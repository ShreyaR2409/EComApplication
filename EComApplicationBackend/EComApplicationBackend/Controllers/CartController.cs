using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using App.Core.App.Cart.Command; 
using App.Core.Models;
using App.Core.App.Cart.Query;
using Domain.Entities;

namespace EComApplicationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int id, [FromBody] CartProductDto CartProductDto)
        {
            var result = await _mediator.Send(new AddToCardCommand {
                Id = id,
                CartDetail = CartProductDto 
            });
      
            {
                return Ok("Item successfully added to cart.");
            }

            return BadRequest("Failed to add item to cart.");
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<GetCartDetailResponse>>> GetCartDetails(int userId)
        {
            var query = new GetCartDetails { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveCartItems(int CartId, int ProductId)
        {
            var result = await _mediator.Send(new RemoveCartItems
            {
                CartId = CartId,
                ProductId = ProductId
            });

            if (!result)
            {
                return BadRequest("Failed to remove the item.");
            }

            return Ok("Item removed successfully.");
        }

        [HttpPost("Card-Details")]
        public async Task<IActionResult> ValidateCardDetails(CardDetails cardDetails)
        {
            var result = await _mediator.Send(new CardDetailsCommand
            {
                CardDetails = cardDetails
            });
            if (!result)
            {
                return NotFound();
            }
            return Ok("Details Matched");
        }

        [HttpGet("generateInvoic/{userId}")]
        public async Task<IActionResult> GenerateInvoice(int userId)
        {
            var addPayment = await _mediator.Send(new GetInvoiceDetailsQuery { SalesId = userId });
            return Ok(addPayment);
        }


    }
}
