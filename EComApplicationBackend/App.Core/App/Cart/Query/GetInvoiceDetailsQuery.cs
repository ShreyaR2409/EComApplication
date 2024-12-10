using App.Core.Interfaces;
using App.Core.Models;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Cart.Query
{
    public class GetInvoiceDetailsQuery : IRequest<object>
    {
        public int SalesId { get; set; }
    }
    public class GetInvoiceDetailsQueryHandler : IRequestHandler<GetInvoiceDetailsQuery, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IConfiguration _configuration;

        public GetInvoiceDetailsQueryHandler(IAppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }

        public async Task<object> Handle(GetInvoiceDetailsQuery request, CancellationToken cancellationToken)
        {
            //var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = _appDbContext.GetConnection();

            // SQL query to join SalesMaster, SalesDetails, Users, and Products
            const string sql = @"
                SELECT 
                    u.id,
                    u.firstname,
                    u.lastname,
                    u.email,
                    u.mobilenumber,
                    u.address AS UserAddress,
                    u.zipcode AS UserZipCode,
                    sm.invoiceid,
                    sm.invoiceDate,
                    sm.total,
                    sm.address,
                    sm.zipcode,
                    sm.state,
                    sm.country,
                    p.productname AS ProductName,
                    sd.ProductCode,
                    sd.quantity,
                    sd.sellingPrice
                FROM SalesMaster sm
                INNER JOIN [users] u ON sm.UserId = u.UserId
                INNER JOIN SalesDetail sd ON sm.SalesId = sd.InvoiceId
                INNER JOIN Products p ON sd.PrId = p.PrId
                WHERE sm.SalesId = @SalesId";

            var parameters = new { SalesId = request.SalesId };

            // Execute the query using Dapper
            var invoiceDetails = await connection.QueryAsync<InvoiceDetailsDto>(sql, parameters);

            // Map the result to a response object
            var groupedInvoice = invoiceDetails
          .GroupBy(x => new
          {
              x.InvoiceId,
              x.UserId,
              x.FirstName,
              x.LastName,
              x.Email,
              x.Mobile,
              x.DeliveryAddress,
              x.DeliveryZipCode,
              x.DeliveryState,
              x.DeliveryCountry,
              x.OrderDate,
              x.TotalAmount
          })
          .Select(g => new
          {
              InvoiceId = g.Key.InvoiceId,
              UserId = g.Key.UserId,
              FirstName = g.Key.FirstName,
              LastName = g.Key.LastName,
              Email = g.Key.Email,
              Mobile = g.Key.Mobile,
              DeliveryAddress = g.Key.DeliveryAddress,
              DeliveryZipCode = g.Key.DeliveryZipCode,
              DeliveryState = g.Key.DeliveryState,
              DeliveryCountry = g.Key.DeliveryCountry,
              OrderDate = g.Key.OrderDate,
              TotalAmount = g.Key.TotalAmount,
              Products = g.Select(p => new
              {
                  ProductName = p.ProductName,
                  ProductCode = p.ProductCode,
                  SalesQty = p.SalesQty,
                  SellingPrice = p.SellingPrice
              }).ToList()
          })
          .FirstOrDefault();

            // Response object
            var response = new
            {
                message = groupedInvoice != null ? "Invoice details retrieved successfully." : "Invoice not found.",
                status = groupedInvoice != null ? 200 : 404,
                invoice = groupedInvoice
            };

            return response;
        }

    }
}
