using App.Core.Interfaces;
using Dapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.User.Query
{
    public class GetUserByUsername : IRequest<Domain.Entities.User>
    {
        public string UserName { get; set; }
    }
    public class GetUserByUsernameHandler : IRequestHandler<GetUserByUsername , Domain.Entities.User>
    {
        private IAppDbContext _appDbContext;
        public GetUserByUsernameHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Domain.Entities.User> Handle(GetUserByUsername request, CancellationToken cancellationToken)
        {
            using var connection = _appDbContext.GetConnection();

            var query = "SELECT * FROM Users WHERE username = @UserName;";

            var user = await connection.QueryFirstOrDefaultAsync<dynamic>(query, new { request.UserName });

            if (user != null)
            {                
                var result = new Domain.Entities.User
                {
                    id = user.id,
                    firstname = user.firstname,
                    lastname = user.lastname,
                    email = user.email,
                    roleid = user.roleid,
                    username = user.username,
                    password = user.password,
                    mobilenumber = user.mobilenumber,
                    profileimage = user.profileimage,
                    address = user.address,
                    zipcode = user.zipcode,
                    countryid = user.countryid,
                    stateid = user.stateid,
                    dob = DateOnly.FromDateTime(user.dob) 
                };

                return result;
            }

            return null;
        }
    }
}
