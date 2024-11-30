using App.Core.Interfaces;
using App.Core.Models;
using MediatR;
using Mapster;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Core.App.User.Command
{
    public class CreateUserCommand : IRequest<RegistrationDto>
    {
        public RegistrationDto Registration { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, RegistrationDto>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;
        public CreateUserCommandHandler(IAppDbContext appDbContext, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }

        public async Task<RegistrationDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var register = request.Registration;

            var existingUser = await _appDbContext.Set< Domain.Entities.User>().FirstOrDefaultAsync(x => x.email == register.email);
            if (existingUser != null)
            {
                //throw new Exception("User already exists");
                return null;
            }

            string username = GenerateUsername(register.firstname, register.lastname, register.dob);
            string password = GenerateRandomPassword();

            var user = register.Adapt<Domain.Entities.User>();
            //user.username = username;
            user.username = username;
            user.password = HashPassword(password);

            await _appDbContext.Set<Domain.Entities.User>().AddAsync(user);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            // Send email with username and password
            string subject = "Welcome to Our Application";
            string body = $"Dear {register.firstname},<br><br>Your registration is successful.<br>Your credentials are:<br>Username: <b>{username}<b><br>Password: <b>{password}<b><br><br>Please change your password after your first login.";
            await _emailService.SendEmailAsync(register.email, subject, body);

            var result = user.Adapt<RegistrationDto>();
            return result;
        }

        private string GenerateUsername(string firstname, string lastname, DateOnly dob)
        {
            string dobFormatted = dob.ToString("ddMMyy");
            string username = $"EC_{lastname.ToUpper()}{firstname[0].ToString().ToUpper()}{dobFormatted}";
            return username;
        }
        private string GenerateRandomPassword()
        {
            const int passwordLength = 8;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, passwordLength).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }

}

