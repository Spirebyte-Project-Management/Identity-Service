using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Identity.Application.Events;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Commands.Handlers
{
    internal sealed class ForgotPasswordHandler : ICommandHandler<ForgotPassword>
    {
        private readonly string Purpose = "resetpassword";
        
        private readonly IUserRepository _userRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IDataProtectorTokenProvider _dataProtector;
        private readonly ILogger<ResetPasswordHandler> _logger;

        public ForgotPasswordHandler(IUserRepository userRepository, IMessageBroker messageBroker, IDataProtectorTokenProvider dataProtector, ILogger<ResetPasswordHandler> logger)
        {
            _userRepository = userRepository;
            _messageBroker = messageBroker;
            _dataProtector = dataProtector;
            _logger = logger;
        }
        public async Task HandleAsync(ForgotPassword command)
        {
            var user = await _userRepository.GetAsync(command.Email);
            if (user is null)
            {
                _logger.LogError($"Email does not exist: {command.Email}");
                throw new InvalidEmailException(command.Email);
            }

            var token = await _dataProtector.GenerateAsync(Purpose, user.Id, user.SecurityStamp);

            await _messageBroker.PublishAsync(new PasswordForgotten(user.Id, user.Fullname, user.Email, token));
        }
    }
}
