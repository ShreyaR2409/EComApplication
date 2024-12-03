using App.Core.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Cart.Command
{
    public class RemoveCartItems : IRequest<bool>
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
    }
    public class RemoveCartItemsHandler : IRequestHandler<RemoveCartItems , bool>
    {
        private readonly IAppDbContext _appDbContext;

        public RemoveCartItemsHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> Handle(RemoveCartItems request, CancellationToken cancellationToken)
        {
            var cartItem = await _appDbContext.Set<CartDetail>()
                .FirstOrDefaultAsync(p => p.cartMasterId == request.CartId && p.productId == request.ProductId, cancellationToken);

            if (cartItem == null)
            {
                return false;
            }

            if (cartItem.quantity > 1)
            {
                cartItem.quantity--;
            }
            else
            {
                _appDbContext.Set<CartDetail>().Remove(cartItem);
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
