using App.Core.App.User.Command;
using App.Core.Interfaces;
using App.Core.Models;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Product.Command
{
    public class AddProductCommand : IRequest<AddProductResponseDto>
    {
        public ProductDto product;
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, AddProductResponseDto>
    {
        private readonly IAppDbContext _appDbContext;
        public AddProductCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<AddProductResponseDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var productDto = request.product;

            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "Product data cannot be null.");
            }

            string imagePath = null;
            if (productDto.productimg != null)
            {
                imagePath = await UploadImagesAsync(productDto.productimg);
            }


            // Validate that selling price is greater than purchase price
            if (productDto.sellingprice <= productDto.purchaseprice)
            {
                return null;
            }

            // Map ProductDto to ProductEntity using Mapster
            var productEntity = productDto.Adapt<Domain.Entities.Product>();

            productEntity.productimg = imagePath;

            // Add product to the database
            await _appDbContext.Set<Domain.Entities.Product>().AddAsync(productEntity);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            // Return the newly created product as ProductDto
            var result = productEntity.Adapt<AddProductResponseDto>();
            return result;
        }

        private async Task<string?> UploadImagesAsync(IFormFile profileimage)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filename = Guid.NewGuid().ToString() + "_" + profileimage.FileName;
            string filePath = Path.Combine(uploadsFolder, filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                profileimage.CopyTo(stream);
            }
            return $"/uploads/{filename}";
        }
    }
}
