using App.Core.App.Product.Command;
using App.Core.App.Product.Query;
using App.Core.App.User.Command;
using App.Core.App.User.Query;
using App.Core.Models;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EComApplicationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize(Roles = "Admin")]

        [HttpPost("Add-Product")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto productDto)
        {
            var result = await _mediator.Send(new AddProductCommand { product = productDto });
            if (result == null)
            {
                return Ok("Selling Price must be greater than Purchase Price.");
            }
            return Ok(result);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("Update-Product/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDto productDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Product ID.");
            }

            if (productDto == null)
            {
                return BadRequest("Product data cannot be null.");
            }

            var result = await _mediator.Send(new UpdateProductCommand
            {
                ProductId = id,
                Product = productDto
            });

            if (result == null)
            {
                return NotFound("Product not found or update failed.");
            }

            return Ok("Product updated successfully.");
        }
        //[Authorize(Roles = "Admin")]

        [HttpDelete("Delete-Product")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand { ProductId = id });
            if (result == null)
            {
                return BadRequest("");
            }
            return Ok(result);
        }

        [HttpGet("GetAll-Product")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _mediator.Send(new GetAllProductQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery{ ProductId = id });
            return Ok(result);
        }
    }
}
