using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Identity.Application.Authentication.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Users.Events;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Users.Commands.Handlers;

internal sealed class ForgotPasswordHandler : ICommandHandler<ForgotPassword>
{
    private readonly IDataProtectorTokenProvider _dataProtector;
    private readonly ILogger<ForgotPasswordHandler> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly string _purpose = "resetpassword";

    private readonly IUserRepository _userRepository;

    public ForgotPasswordHandler(IUserRepository userRepository, IMessageBroker messageBroker,
        IDataProtectorTokenProvider dataProtector, ILogger<ForgotPasswordHandler> logger)
    {
        _userRepository = userRepository;
        _messageBroker = messageBroker;
        _dataProtector = dataProtector;
        _logger = logger;
    }

    public async Task HandleAsync(ForgotPassword command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(command.Email);
        if (user is null)
        {
            _logger.LogError($"Email does not exist: {command.Email}");
            throw new InvalidEmailException(command.Email);
        }

        var token = await _dataProtector.GenerateAsync(_purpose, user.Id, user.SecurityStamp);

        await _messageBroker.PublishAsync(new PasswordForgotten(user.Id, user.Fullname, user.Email, token));
    }
}