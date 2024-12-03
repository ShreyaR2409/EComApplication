using App.Core.Interfaces;
using App.Core.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.App.User.Command
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public RegistrationDto Registration { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IAppDbContext _appDbContext;

        public UpdateUserCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userDto = request.Registration;

            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto), "User data cannot be null.");
            }

            var existingUser = await _appDbContext.Set<Domain.Entities.User>()
                .FirstOrDefaultAsync(u => u.id == request.UserId, cancellationToken);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
            }

            // Update existing user properties
            existingUser.firstname = userDto.firstname;
            existingUser.lastname = userDto.lastname;
            existingUser.email = userDto.email;
            existingUser.dob = userDto.dob;

            // Handle profile image upload if provided
            if (userDto.profileimage != null)
            {
                string newImagePath = await UploadImagesAsync(userDto.profileimage);
                existingUser.profileimage = newImagePath;
            }

            // Update the user in the database
            _appDbContext.Set<Domain.Entities.User>().Update(existingUser);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return true; // Indicate successful update
        }

        private async Task<string?> UploadImagesAsync(IFormFile profileimage)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filename = Guid.NewGuid().ToString() + "_" + profileimage.FileName;
            string filePath = Path.Combine(uploadsFolder, filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profileimage.CopyToAsync(stream);
            }

            return $"/uploads/{filename}";
        }
    }
}