﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spirebyte.Services.Identity.Application.DTO;
using Spirebyte.Services.Identity.Application.Exceptions;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Entities;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Core.Exceptions;
using Spirebyte.Services.Identity.Core.Repositories;

namespace Spirebyte.Services.Identity.Application.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRng _rng;
    private readonly IUserRepository _userRepository;

    public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository, IJwtProvider jwtProvider, IRng rng)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _rng = rng;
    }

    public async Task<string> CreateAsync(Guid userId)
    {
        var token = _rng.Generate(30);
        var refreshToken = new RefreshToken(new AggregateId(), userId, token, DateTime.UtcNow);
        await _refreshTokenRepository.AddAsync(refreshToken);

        return token;
    }

    public async Task RevokeAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetAsync(refreshToken);
        if (token is null) throw new InvalidRefreshTokenException();

        token.Revoke(DateTime.UtcNow);
        await _refreshTokenRepository.UpdateAsync(token);
    }

    public async Task<AuthDto> UseAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetAsync(refreshToken);
        if (token is null) throw new InvalidRefreshTokenException();

        if (token.Revoked) throw new RevokedRefreshTokenException();

        var user = await _userRepository.GetAsync(token.UserId);
        if (user is null) throw new UserNotFoundException(token.UserId);

        var claims = user.Permissions.Any()
            ? new Dictionary<string, IEnumerable<string>>
            {
                ["permissions"] = user.Permissions
            }
            : null;
        var auth = _jwtProvider.Create(token.UserId, user.Role, claims: claims);
        auth.RefreshToken = refreshToken;

        return auth;
    }
}