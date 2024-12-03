using App.Core.Interfaces;
using App.Core.Models;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace App.Core.App.Cart.Command
{
    public class AddToCardCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public CartProductDto CartDetail { get; set; }
    }

    public class AddToCardCommandHandler : IRequestHandler<AddToCardCommand, bool>
    {
        private readonly IAppDbContext _appDbContext;

        public AddToCardCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> Handle(AddToCardCommand request, CancellationToken cancellationToken)
        {
            var cartDetail = request.CartDetail;
            if (cartDetail == null)
            {
                return false; 
            }

            // Find an existing cart for the user
            var existingCart = await _appDbContext.Set<CartMaster>()
                .Where(c => c.userId == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingCart == null)
            {
              
                var newCart = new CartMaster
                {
                    userId = request.Id
                };
                await _appDbContext.Set<CartMaster>().AddAsync(newCart, cancellationToken);
                await _appDbContext.SaveChangesAsync(cancellationToken);

           
                var newCartProduct = new CartDetail
                {
                    cartMasterId = newCart.Id,
                    productId = cartDetail.productId,
                    quantity = cartDetail.quantity
                };
                await _appDbContext.Set<CartDetail>().AddAsync(newCartProduct, cancellationToken);
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
            
                var existingProduct = await _appDbContext.Set<CartDetail>()
                    .Where(cd => cd.cartMasterId == existingCart.Id && cd.productId == cartDetail.productId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingProduct == null)
                {
                  
                    var newProductInCart = new CartDetail
                    {
                        cartMasterId = existingCart.Id,
                        productId = cartDetail.productId,
                        quantity = cartDetail.quantity
                    };
                    await _appDbContext.Set<CartDetail>().AddAsync(newProductInCart, cancellationToken);
                    await _appDbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                   
                    existingProduct.quantity += cartDetail.quantity;
                    await _appDbContext.SaveChangesAsync(cancellationToken);
                }
            }

            return true;
        }
    }
}
