using App.Core.App.User.Query;
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
    public class GetAllProductQuery : IRequest<List<Domain.Entities.Product>>
    {
    }
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, List<Domain.Entities.Product>>
    {
        private readonly IAppDbContext _appDbContext;
        public GetAllProductQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Domain.Entities.Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            using var connection = _appDbContext.GetConnection();

            // Querying products that are not soft-deleted
            var query = "SELECT * FROM products WHERE IsDeleted = 0;";

            var data = await connection.QueryAsync<Domain.Entities.Product>(query);

            return data.ToList(); // Convert IEnumerable to List
        }

    }
}
