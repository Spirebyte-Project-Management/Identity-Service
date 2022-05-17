using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Identity.Application.Authentication.Services.Interfaces;
using Spirebyte.Services.Identity.Application.Users.Events;
using Spirebyte.Services.Identity.Application.Users.Exceptions;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Users.Commands.Handlers;

// Simple wrapper
internal sealed class SignUpHandler : ICommandHandler<SignUp>
{
    private static readonly Regex EmailRegex = new(
        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly ILogger<SignUpHandler> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;

    public SignUpHandler(IUserRepository userRepository, IMessageBroker messageBroker, UserManager<User> userManager, ILogger<SignUpHandler> logger)
    {
        _userRepository = userRepository;
        _messageBroker = messageBroker;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task HandleAsync(SignUp command, CancellationToken cancellationToken = default)
    {
        if (!EmailRegex.IsMatch(command.Email))
        {
            _logger.LogError("Invalid email: {Email}", command.Email);
            throw new InvalidEmailException(command.Email);
        }

        var existingUser = await _userRepository.GetAsync(command.Email);
        if (existingUser is { })
        {
            _logger.LogError("Email already in use: {Email}", command.Email);
            throw new EmailInUseException(command.Email);
        }

        var user = new User
        {
            UserName = command.Fullname,
            Email = command.Email
        };
        
        var identityResult = await _userManager.CreateAsync(user, command.Password);

        if (!identityResult.Succeeded)
        {
            var firstError = identityResult.Errors.First();
            throw new SignUpException(firstError.Code, firstError.Description);
        }
        _logger.LogInformation($"Created an account for the user with id: {user.Id}.");
        await _messageBroker.PublishAsync(new SignedUp(user.Id, user.Email));
    }
}