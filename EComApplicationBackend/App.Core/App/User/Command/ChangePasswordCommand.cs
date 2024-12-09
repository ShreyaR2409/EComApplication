using App.Core.Interfaces;
using App.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.User.Command
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        //public string UserId { get; set; } 
        public ChangePasswordDto ChangePasswordDto { get; set; }
    }
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IAppDbContext _appDbContext;

        public ChangePasswordCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ChangePasswordDto;

            var user = await _appDbContext.Set<Domain.Entities.User>()
                .FirstOrDefaultAsync(u => u.id == dto.UserId, cancellationToken);

            if (user == null)
            {
                return false; 
            }

            if (BCrypt.Net.BCrypt.Verify(dto.NewPassword, user.password))
            {
                return false; 
            }

      
            user.password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            _appDbContext.Set<Domain.Entities.User>().Update(user);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return true; 
        }
    }
}