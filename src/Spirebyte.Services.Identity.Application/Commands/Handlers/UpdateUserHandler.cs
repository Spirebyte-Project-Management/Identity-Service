using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Identity.Application.Exceptions;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Commands.Handlers
{
    internal sealed class UpdateUserHandler : ICommandHandler<UpdateUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserHandler> _logger;

        public UpdateUserHandler(IUserRepository userRepository, ILogger<UpdateUserHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task HandleAsync(UpdateUser command)
        {

            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            user = new User(user.Id, user.Email, command.Fullname, command.Pic, user.Password, user.Role, user.CreatedAt, user.Permissions);
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Updated account for the user with id: {user.Id}.");
        }
    }
}
