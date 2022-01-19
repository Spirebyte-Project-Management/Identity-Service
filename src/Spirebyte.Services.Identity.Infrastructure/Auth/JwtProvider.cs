using System;
using System.Collections.Generic;
using Convey.Auth;
using Spirebyte.Services.Identity.Application.DTO;
using Spirebyte.Services.Identity.Application.Services.Interfaces;

namespace Spirebyte.Services.Identity.Infrastructure.Auth;

public class JwtProvider : IJwtProvider
{
    private readonly IJwtHandler _jwtHandler;

    public JwtProvider(IJwtHandler jwtHandler)
    {
        _jwtHandler = jwtHandler;
    }

    public AuthDto Create(Guid userId, string role, string audience = null,
        IDictionary<string, IEnumerable<string>> claims = null)
    {
        var jwt = _jwtHandler.CreateToken(userId.ToString("N"), role, audience, claims);

        return new AuthDto
        {
            AccessToken = jwt.AccessToken,
            Role = jwt.Role,
            Expires = jwt.Expires
        };
    }
}