using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Identity.Application.Authentication.Exceptions;
using Spirebyte.Services.Identity.Application.Authentication.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Users.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Users.Commands.Handlers;

internal sealed class ResetPasswordHandler : ICommandHandler<ResetPassword>
{
    private readonly IDataProtectorTokenProvider _dataProtector;
    private readonly ILogger<ResetPasswordHandler> _logger;
    private readonly IPasswordService _passwordService;
    private readonly string _purpose = "resetpassword";

    private readonly IUserRepository _userRepository;

    public ResetPasswordHandler(IUserRepository userRepository, IPasswordService passwordService,
        IDataProtectorTokenProvider dataProtector,
        ILogger<ResetPasswordHandler> logger)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _dataProtector = dataProtector;
        _logger = logger;
    }

    public async Task HandleAsync(ResetPassword command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(command.UserId);
        if (user is null) throw new UserNotFoundException(command.UserId);

        var token = await _dataProtector.ValidateAsync(_purpose, command.Token, user.Id, user.SecurityStamp);
        if (token == false) throw new InvalidTokenException(command.UserId);

        var password = _passwordService.Hash(command.Password);
        user.SetPassword(password);

        await _userRepository.UpdateAsync(user);
        _logger.LogInformation("Updated password for the user with id: {user.Id}.");
    }
}