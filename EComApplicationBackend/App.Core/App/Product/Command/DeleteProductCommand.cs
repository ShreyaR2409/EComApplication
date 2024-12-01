using App.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Product.Command
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public int ProductId { get; set; }
    }
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand , bool>
    {
        private readonly IAppDbContext _appDbContext;
        public DeleteProductCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _appDbContext.Set<Domain.Entities.Product>()
                .FirstOrDefaultAsync(p => p.id == request.ProductId && !p.isDeleted, cancellationToken);

            if (product == null)
            {
                return false; // Product not found or already deleted
            }

            // Perform soft delete
            product.isDeleted = true;
            //product.UpdatedAt = DateTime.UtcNow;

            _appDbContext.Set<Domain.Entities.Product>().Update(product);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

