using App.Core.Interfaces;
using App.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.App.User.Command
{
    public class VerifyOtpCommand : IRequest<JwtResponseDto>
    {
        public VerifyOtpDto VerifyOtp { get; set; }
    }

    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, JwtResponseDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IJwtService _jwtService;

        public VerifyOtpCommandHandler(IAppDbContext appDbContext, IJwtService jwtService)
        {
            _appDbContext = appDbContext;
            _jwtService = jwtService;
        }

        public async Task<JwtResponseDto> Handle(VerifyOtpCommand request, CancellationToken token)
        {
            var verifyOtpModel = request.VerifyOtp;

            // Retrieve the user using the username
            var user = await _appDbContext.Set<Domain.Entities.User>()
                .FirstOrDefaultAsync(x => x.username == verifyOtpModel.Username);

            if (user == null)
            {
                return null; // User not found
            }

            // Now use the userId to verify the OTP
            var otpEntity = await _appDbContext.Set<Domain.Entities.Otp>()
                .FirstOrDefaultAsync(x => x.userid == user.id && x.otp == verifyOtpModel.Otp);

            if (otpEntity == null)
            {
                return null; 
            }

            var role = await _appDbContext.Set<Domain.Entities.Role>()
        .FirstOrDefaultAsync(r => r.id == user.roleid);

            if (role == null)
            {
                return null; // Role not found
            }

            // Generate JWT token for the user
            return new JwtResponseDto
            {
                Token = _jwtService.GenerateToken(user, role.roletype)
            };
        }

    }
}
