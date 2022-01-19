using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Identity.Application.Exceptions;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Commands.Handlers;

internal sealed class ResetPasswordHandler : ICommandHandler<ResetPassword>
{
    private readonly IDataProtectorTokenProvider _dataProtector;
    private readonly ILogger<ResetPasswordHandler> _logger;
    private readonly IPasswordService _passwordService;

    private readonly IUserRepository _userRepository;
    private readonly string Purpose = "resetpassword";

    public ResetPasswordHandler(IUserRepository userRepository, IPasswordService passwordService,
        IDataProtectorTokenProvider dataProtector,
        ILogger<ResetPasswordHandler> logger)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _dataProtector = dataProtector;
        _logger = logger;
    }

    public async Task HandleAsync(ResetPassword command)
    {
        var user = await _userRepository.GetAsync(command.UserId);
        if (user is null) throw new UserNotFoundException(command.UserId);

        var token = await _dataProtector.ValidateAsync(Purpose, command.Token, user.Id, user.SecurityStamp);
        if (token == false) throw new InvalidTokenException(command.UserId);

        var password = _passwordService.Hash(command.Password);
        user.SetPassword(password);

        await _userRepository.UpdateAsync(user);
        _logger.LogInformation("Updated password for the user with id: {user.Id}.");
    }
}