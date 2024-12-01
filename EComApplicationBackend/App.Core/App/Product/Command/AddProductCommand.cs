using App.Core.App.User.Command;
using App.Core.Interfaces;
using App.Core.Models;
using Domain.Entities;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Product.Command
{
    public class AddProductCommand : IRequest<ProductDto>
    {
        public ProductDto product;
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ProductDto>
    {
        private readonly IAppDbContext _appDbContext;
        public AddProductCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ProductDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var productDto = request.product;

            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "Product data cannot be null.");
            }

            // Validate that selling price is greater than purchase price
            if (productDto.sellingprice <= productDto.purchaseprice)
            {
                throw new ArgumentException("Selling Price must be greater than Purchase Price.");
            }

            // Map ProductDto to ProductEntity using Mapster
            var productEntity = productDto.Adapt<Domain.Entities.Product>();

            //productEntity.CreatedAt = DateTime.UtcNow;
            //productEntity.UpdatedAt = DateTime.UtcNow;

            // Add product to the database
            await _appDbContext.Set<Domain.Entities.Product>().AddAsync(productEntity);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            // Return the newly created product as ProductDto
            var result = productEntity.Adapt<ProductDto>();
            return result;
        }
    }
}
