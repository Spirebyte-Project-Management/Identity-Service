using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Convey.WebApi.Requests;
using Microsoft.Extensions.Logging;
using Spirebyte.Services.Identity.Application.DTO;
using Spirebyte.Services.Identity.Application.Events;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Requests.Handlers
{
    public class SignInHandler : IRequestHandler<SignIn, AuthDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<SignInHandler> _logger;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenService _refreshTokenService;

        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);


        public SignInHandler(IUserRepository userRepository, IPasswordService passwordService,
            IMessageBroker messageBroker, ILogger<SignInHandler> logger,
            IJwtProvider jwtProvider, IRefreshTokenService refreshTokenService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _messageBroker = messageBroker;
            _logger = logger;
            _jwtProvider = jwtProvider;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<AuthDto> HandleAsync(SignIn request)
        {
            if (!EmailRegex.IsMatch(request.Email))
            {
                _logger.LogWarning($"Invalid email: {request.Email}");
                throw new InvalidEmailException(request.Email);
            }

            var user = await _userRepository.GetAsync(request.Email);
            if (user is null)
            {
                _logger.LogWarning($"User with email: {request.Email} was not found.");
                throw new InvalidCredentialsException(request.Email);
            }

            if (user.IsLockedOut)
            {
                throw new UserLockedOutException(user.LockoutEnd.Subtract(DateTime.Now).Hours); 
            }

            if (!_passwordService.IsValid(user.Password, request.Password))
            {
                _logger.LogWarning($"Invalid authentication for user with id: {user.Id.Value}", user);
                user.InvalidLogin();
                await _userRepository.UpdateAsync(user);

                if (user.IsLockedOut)
                {
                    _logger.LogWarning($"Locked out user with Id: {user.Id.Value}", user);

                    throw new UserLockedOutException(user.LockoutEnd.Subtract(DateTime.Now).Hours);
                }

                throw new InvalidCredentialsException(request.Email);
            }

            var claims = user.Permissions.Any()
                ? new Dictionary<string, IEnumerable<string>>
                {
                    ["permissions"] = user.Permissions
                }
                : null;
            var auth = _jwtProvider.Create(user.Id, user.Role, claims: claims);
            auth.RefreshToken = await _refreshTokenService.CreateAsync(user.Id);

            user.ValidLogin();
            await _userRepository.UpdateAsync(user);


            _logger.LogInformation($"User with id: {user.Id} has been authenticated.", user);
            await _messageBroker.PublishAsync(new SignedIn(user.Id, user.Role));

            return auth;
        }
    }
}
