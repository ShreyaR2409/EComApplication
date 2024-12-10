using App.Core.Interfaces;
using App.Core.Models;
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
    public class AddPaymentCommand : IRequest<object>
    {
        public CartPaymentDto CartPaymentDto { get; set; }
    }

    internal class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, object>
    {
        private readonly IAppDbContext _appDbContext;

        public AddPaymentCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
        {
            var paymentAndOrderDto = request.CartPaymentDto;

            //Validate card
            var card = await _appDbContext.Set<Domain.Entities.CardDetails>()
                            .FirstOrDefaultAsync(c => c.cardnumber== paymentAndOrderDto.CardNumber &&
                            c.cvv == paymentAndOrderDto.Cvv, cancellationToken);

            if (card is null)
            {
                return new { status = 404, message = "Item not found in the cart", data = card };
            }


            if (card.expirydate.ToString("MM/dd/yyyy") != paymentAndOrderDto.ExpiryDate?.ToString("MM/dd/yyyy"))
            {
                return "Wrong Expiry Date";
            }




            var cartDetailsList = await (from cartMaster in _appDbContext.Set<Domain.Entities.CartMaster>()
                                         join cartDetail in _appDbContext.Set<Domain.Entities.CartDetail>()
                                         on cartMaster.Id equals cartDetail.cartMasterId
                                         where (cartMaster.userId == paymentAndOrderDto.UserId)
                                         select new Domain.Entities.CartDetail
                                         {
                                             //Id = cartDetail.Id,
                                             Id = cartDetail.Id,
                                             cartMaster = cartDetail.cartMaster,
                                             Product = cartDetail.Product,
                                             quantity = cartDetail.quantity,
                                             productId = cartDetail.productId,
                                         }).ToListAsync(cancellationToken: cancellationToken);

            if (!cartDetailsList.Any())
                return $"No Item in Cart";

            //var cartProducntList = new List<Domain.Entities.Product>();
            float subTotal = 0;
            foreach (var item in cartDetailsList)
            {
                var product = await _appDbContext.Set<Domain.Entities.Product>()
                                    .FirstOrDefaultAsync(p => p.id == item.productId, cancellationToken: cancellationToken);

                if (product is null || !(product.Stock >= item.quantity))
                {
                    return new { status = 404, message = "Item not in the stock", data = product };
                }

                // Find Subtotal
                subTotal += product.sellingprice * (item.quantity);
                //cartProducntList.Add(product);
            }
            int totalSalesMasterEntity = await _appDbContext.Set<Domain.Entities.SalesMaster>().
                                          CountAsync(cancellationToken: cancellationToken);
            totalSalesMasterEntity++;
            // Add entry to SalesMaster Table
            SalesMaster salesMaster = new SalesMaster()
            {
                invoiceDate = DateTime.Now,
                //total = subTotal,
                address = paymentAndOrderDto.Address,
                state = paymentAndOrderDto.StateName,
                country = paymentAndOrderDto.CountryName,
                zipcode = paymentAndOrderDto.ZipCode,
                userid = paymentAndOrderDto.UserId,
                invoiceId = "ORD" + totalSalesMasterEntity.ToString().PadLeft(3, '0'),
            };



            // Add Entry in SalesMaster Table 
            await _appDbContext.Set<Domain.Entities.SalesMaster>()
                  .AddAsync(salesMaster, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            salesMaster.invoiceId = "ORDER" + salesMaster.id.ToString().PadLeft(3, '0');
            await _appDbContext.SaveChangesAsync(cancellationToken);

            foreach (var item in cartDetailsList)
            {
                var product = await _appDbContext.Set<Domain.Entities.Product>()
                                   .FirstOrDefaultAsync(p => p.id == item.productId, cancellationToken: cancellationToken);

                SalesDetail salesDetails = new SalesDetail()
                {
                    invoiceId = salesMaster.id,
                    productCode = product.productcode,
                    id = product.id,
                    quantity = item.quantity,
                    salesMaster = salesMaster,
                    sellingPrice = product.sellingprice,

                };

                await _appDbContext.Set<Domain.Entities.SalesDetail>()
                       .AddAsync(salesDetails, cancellationToken);

                product.Stock = product.Stock - item.quantity;

                _appDbContext.Set<Domain.Entities.CartDetail>()
                    .Remove(item);

                // Saving the all changes
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }


            var response = new
            {
                status = 200,
                message = "Card Order Placed Successfully",
                data = salesMaster
            };
            return response;


        }
    }
}
