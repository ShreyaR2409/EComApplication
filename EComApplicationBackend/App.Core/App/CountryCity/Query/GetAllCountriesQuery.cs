using App.Core.Interfaces;
using Dapper;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.CountryCity.Query
{
    public class GetAllCountriesQuery : IRequest<List<Country>>
    {
    }

    public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, List<Country>>
    {
        private readonly IAppDbContext _appDbContext;
        public GetAllCountriesQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Country>> Handle(GetAllCountriesQuery request, CancellationToken token)
        {
            using var connection = _appDbContext.GetConnection();

            var query = "SELECT * FROM Countries;";

            var data = await connection.QueryAsync<Country>(query);

            return data.AsList();
        }

    }
}
