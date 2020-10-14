using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Identity.Application.Exceptions;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Commands.Handlers
{
    internal sealed class ResetPasswordHandler : ICommandHandler<ResetPassword>
    {
        private readonly string Purpose = "resetpassword";
        
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IMessageBroker _messageBroker;
        private readonly IDataProtectorTokenProvider _dataProtector;
        private readonly ILogger<ResetPasswordHandler> _logger;

        public ResetPasswordHandler(IUserRepository userRepository, IPasswordService passwordService,
            IMessageBroker messageBroker, IDataProtectorTokenProvider dataProtector,
            ILogger<ResetPasswordHandler> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _messageBroker = messageBroker;
            _dataProtector = dataProtector;
            _logger = logger;
        }
        public async Task HandleAsync(ResetPassword command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            var token = await _dataProtector.ValidateAsync(Purpose, command.Token, user.Id, user.SecurityStamp);
            if (token == false)
            {
                throw new InvalidTokenException(command.UserId);
            }

            var password = _passwordService.Hash(command.Password);
            user.SetPassword(password);

            await _userRepository.UpdateAsync(user);
            _logger.LogInformation("Updated password for the user with id: {user.Id}.");
        }
    }
}
