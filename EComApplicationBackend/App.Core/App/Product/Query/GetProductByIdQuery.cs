using App.Core.Interfaces;
using Dapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Product.Query
{
    public class GetProductByIdQuery : IRequest<Domain.Entities.Product>
    {
        public int ProductId { get; set; }
    }
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Domain.Entities.Product>
    {
        private readonly IAppDbContext _appDbContext;

        public GetProductByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Domain.Entities.Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = _appDbContext.GetConnection();

            var query = "SELECT * FROM Products WHERE id = @ProductId AND isDeleted = 0;";

            var product = await connection.QueryFirstOrDefaultAsync<Domain.Entities.Product>(query, new { request.ProductId });

            return product; 
        }
    }
}
