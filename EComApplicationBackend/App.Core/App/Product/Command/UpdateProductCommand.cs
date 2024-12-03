using App.Core.Interfaces;
using App.Core.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Product.Command
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
    }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IAppDbContext _appDbContext;

        public UpdateProductCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productDto = request.Product;

            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "Product data cannot be null.");
            }

            var existingProduct = await _appDbContext.Set<Domain.Entities.Product>()
                .FirstOrDefaultAsync(p => p.id == request.ProductId, cancellationToken);

            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");
            }

            // Update existing product properties
            existingProduct.productname = productDto.productname;
            existingProduct.purchaseprice = productDto.purchaseprice;
            existingProduct.brand = productDto.brand;
            existingProduct.sellingprice = productDto.sellingprice;
            existingProduct.category = productDto.category;
            existingProduct.Stock = productDto.Stock;
            existingProduct.purchasedate = productDto.purchasedate;
            existingProduct.productcode = productDto.productcode;
            // Handle image upload if provided
            if (productDto.productimg != null)
            {
                string newImagePath = await UploadImagesAsync(productDto.productimg);
                existingProduct.productimg = newImagePath; // Update the product image URL
            }

            // Update the product in the database
            _appDbContext.Set<Domain.Entities.Product>().Update(existingProduct);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return true; // Indicate successful update
        }

        private async Task<string?> UploadImagesAsync(IFormFile productImage)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filename = Guid.NewGuid().ToString() + "_" + productImage.FileName;
            string filePath = Path.Combine(uploadsFolder, filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await productImage.CopyToAsync(stream);
            }

            return $"/uploads/{filename}"; // Return the relative URL to the uploaded image
        }
    }
}