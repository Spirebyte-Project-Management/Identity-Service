using System.Threading.Tasks;
using Convey.WebApi.Requests;
using Spirebyte.Services.Identity.Application.DTO;
using Spirebyte.Services.Identity.Application.Exceptions;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Requests.Handlers
{
    internal sealed class UseRefreshTokenHandler : IRequestHandler<UseRefreshToken, AuthDto>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public UseRefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository,
            IJwtProvider jwtProvider)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<AuthDto> HandleAsync(UseRefreshToken request)
        {
            var token = await _refreshTokenRepository.GetAsync(request.RefreshToken);
            if (token is null)
            {
                throw new InvalidRefreshTokenException();
            }

            if (token.Revoked)
            {
                throw new RevokedRefreshTokenException();
            }

            var user = await _userRepository.GetAsync(token.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(token.UserId);
            }

            var auth = _jwtProvider.Create(token.UserId, user.Role);
            auth.RefreshToken = request.RefreshToken;

            return auth;
        }
    }
}
