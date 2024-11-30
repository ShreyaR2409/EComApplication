using App.Core.Interfaces;
using Dapper;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.CountryCity.Query
{
    public class GetStatesByCountryIdQuery : IRequest<List<State>>
    {
        public int Id { get; set; }
    }
    public class GetAllStatesQueryHandler : IRequestHandler<GetStatesByCountryIdQuery, List<State>>
    {
        private readonly IAppDbContext _appDbContext;
        public GetAllStatesQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<State>> Handle(GetStatesByCountryIdQuery request, CancellationToken token)
        {
            using var connection = _appDbContext.GetConnection();
            var query = "SELECT * FROM States WHERE countryid = @Id;";
            var data = await connection.QueryAsync<State>(query, new { Id = request.Id });
            return data.AsList();
        }
    }
}
