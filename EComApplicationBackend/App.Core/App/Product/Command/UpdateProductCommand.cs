using App.Core.Interfaces;
using App.Core.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Product.Command
{
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
    }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IAppDbContext _appDbContext;

        public UpdateProductCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productDto = request.Product;

            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "Product data cannot be null.");
            }

            // Fetch the existing product from the database
            var existingProduct = await _appDbContext.Set<Domain.Entities.Product>()
                .FirstOrDefaultAsync(p => p.id == request.ProductId, cancellationToken);

            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");
            }

            // Validate that selling price is greater than purchase price
            if (productDto.sellingprice <= productDto.purchaseprice)
            {
                throw new ArgumentException("Selling Price must be greater than Purchase Price.");
            }

            // Update the existing product entity with new values using Mapster
            productDto.Adapt(existingProduct);

            //existingProduct.UpdatedAt = DateTime.UtcNow; // Update the timestamp

            // Save the changes to the database
            _appDbContext.Set<Domain.Entities.Product>().Update(existingProduct);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            // Return the updated product as ProductDto
            var result = existingProduct.Adapt<ProductDto>();
            return result;
        }
    }
}
