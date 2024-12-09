using App.Core.Interfaces;
using App.Core.Models;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace App.Core.App.Cart.Query
{
    public class GetCartDetails : IRequest<List<GetCartDetailResponse>>
    {
        public int UserId { get; set; }
    }

    public class GetCartDetailsHandler : IRequestHandler<GetCartDetails, List<GetCartDetailResponse>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetCartDetailsHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<GetCartDetailResponse>> Handle(GetCartDetails request, CancellationToken cancellationToken)
        {
            using var connection = _appDbContext.GetConnection();
            var query = "sp_getCartDetails";
            var dbParams = new
            {
                Id = request.UserId
            };
            var data = await connection.QueryAsync<GetCartDetailResponse>
            (
                query,
                dbParams,
                commandType: CommandType.StoredProcedure
            );

            return data.ToList();
        }
    }
}
