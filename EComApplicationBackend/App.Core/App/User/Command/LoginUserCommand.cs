using App.Core.Interfaces;
using App.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.App.User.Command
{
    public class LoginUserCommand : IRequest<LoginResponseDto>
    {
        public LoginDto Login {  get; set; }
    
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponseDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;

        public LoginUserCommandHandler(IAppDbContext appDbContext, IEmailService emailService, IJwtService jwtService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
            _jwtService = jwtService;   
        }

        public async Task<LoginResponseDto> Handle(LoginUserCommand request, CancellationToken token)
        {
            var LoginModel = request.Login;
            var user = await _appDbContext.Set<Domain.Entities.User>().FirstOrDefaultAsync(x => x.username == LoginModel.username);
            if (user == null || !VerifyPassword(LoginModel.password, user.password))
            {
                return null;
            }

            var otp = GenerateOtp();
            var existingOtp = await _appDbContext.Set<Domain.Entities.Otp>().FirstOrDefaultAsync(x => x.userid == user.id, token);

            //var otpEntity = new Domain.Entities.Otp
            //{
            //    userid = user.id,
            //    otp = otp
            //};
            //await _appDbContext.Set<Domain.Entities.Otp>().AddAsync(otpEntity);
            //await _appDbContext.SaveChangesAsync();
            if (existingOtp != null)
            {
                // Update the existing OTP entry
                existingOtp.otp = otp;
                //existingOtp.expiresAt = DateTime.UtcNow.AddMinutes(5); // Update expiry time
                _appDbContext.Set<Domain.Entities.Otp>().Update(existingOtp);
            }
            else
            {
                // Create a new OTP entry
                var otpEntity = new Domain.Entities.Otp
                {
                    userid = user.id,
                    otp = otp,
                    //expiresAt = DateTime.UtcNow.AddMinutes(5)
                };
                await _appDbContext.Set<Domain.Entities.Otp>().AddAsync(otpEntity, token);
            }

            await _appDbContext.SaveChangesAsync(token);

            string subject = "Your OTP Code";
            string body = $"Dear {user.username},<br><br>Your OTP code is: <b>{otp}</b><br><br>Please use this code to complete your login.";
            await _emailService.SendEmailAsync(user.email, subject, body);
            return new LoginResponseDto
            {
                Otp = otp
            };
        }


        public bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
        }

        public string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
