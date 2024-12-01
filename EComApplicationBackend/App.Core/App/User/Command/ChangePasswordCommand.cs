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
        public string UserId { get; set; } // Get this from the authenticated user's token/session
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
            int userId = int.Parse(request.UserId);
            var newPassword = request.ChangePasswordDto.NewPassword;

            // Fetch user by userId
            var user = await _appDbContext.Set<Domain.Entities.User>()
                .FirstOrDefaultAsync(u => u.id == userId, cancellationToken);

            if (user == null)
            {
                return false; // User not found
            }

            // Verify if the new password matches the current password
            if (BCrypt.Net.BCrypt.Verify(newPassword, user.password))
            {
                return false; // New password is the same as the current password
            }

            // Hash and update the new password
            user.password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _appDbContext.Set<Domain.Entities.User>().Update(user);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return true; // Password updated successfully
        }
    }

}
