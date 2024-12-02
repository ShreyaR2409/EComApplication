using App.Core.Interfaces;
using App.Core.Models;
using Mapster;
using MediatR;
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

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationtoken)
        {
            var UserDto = request.Registration;
            if (UserDto == null)
            {
                throw new ArgumentNullException(nameof(UserDto), "User data cannot be null.");
            }

            var existingUser = await _appDbContext.Set<Domain.Entities.User>()
         .FirstOrDefaultAsync(p => p.id == request.UserId);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
            }

            UserDto.Adapt(existingUser);

            // Save the changes to the database
            _appDbContext.Set<Domain.Entities.User>().Update(existingUser);
            await _appDbContext.SaveChangesAsync(cancellationtoken);

            // Return the updated product as ProductDto
            var result = existingUser.Adapt<bool>();
            return result;
        }

    }
    }

