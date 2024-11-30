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
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public ForgotPasswordDto ForgotPassword { get; set; }
    }
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(IAppDbContext appDbContext, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var email = request.ForgotPassword.Email;
            var user = await _appDbContext.Set<Domain.Entities.User>()
                .FirstOrDefaultAsync(u => u.email == email, cancellationToken);

            if (user == null)
            {
                return false; // Email not found
            }

            // Generate new random password
            var newPassword = GenerateRandomPassword(8);
            user.password = HashPassword(newPassword); // Hash the new password before saving

            // Update the user's password in the database
            _appDbContext.Set<Domain.Entities.User>().Update(user);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            // Send the new password via email
            string subject = "Your New Password";
            string body = $@"
                Dear {user.username},<br><br>
                Your new password is: <b>{newPassword}</b><br>
                Please use this password to log in and reset it as soon as possible.";
            await _emailService.SendEmailAsync(user.email, subject, body);

            return true;
        }

        private string GenerateRandomPassword(int length)
        {
            const string validChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
            var random = new Random();
            var password = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                password.Append(validChars[random.Next(validChars.Length)]);
            }

            return password.ToString();
        }

        private string HashPassword(string plainTextPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
        }
    }
}
