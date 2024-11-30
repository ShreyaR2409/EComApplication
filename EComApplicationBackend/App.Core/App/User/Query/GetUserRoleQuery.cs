using App.Core.Interfaces;
using Dapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.User.Query
{
    public class GetUserRoleQuery : IRequest<List<Role>>
    {
    }
    public class GetUserRoleQueryHandler : IRequestHandler<GetUserRoleQuery, List<Role>>
    {
       private readonly IAppDbContext _appDbContext;
        public GetUserRoleQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;   
        }

        public async Task<List<Role>> Handle(GetUserRoleQuery request, CancellationToken cancellationToken)
        {
            //var list = await _appDbContext.Set<Role>().AsNoTracking().ToListAsync();
            //return list;
            using var connection = _appDbContext.GetConnection();

            var query = "SELECT * FROM roles;";

            var data = await connection.QueryAsync<Role>(query);

            return data.AsList();
        }
    }
}
