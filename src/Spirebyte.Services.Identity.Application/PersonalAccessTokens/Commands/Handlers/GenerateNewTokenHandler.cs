using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Convey.Auth;
using Convey.CQRS.Commands;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Spirebyte.Services.Identity.Application.PersonalAccessTokens.Services.Interfaces;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Identity.Application.PersonalAccessTokens.Commands.Handlers;

public class GenerateNewTokenHandler : ICommandHandler<GenerateNewToken>
{
    private readonly ITokenService _tokenService;
    private readonly ISystemClock _clock;
    private readonly IAppContext _appContext;
    private readonly ITokenRequestStorage _tokenRequestStorage;
    private readonly JwtOptions _jwtOptions;

    public GenerateNewTokenHandler(ITokenService tokenService, ISystemClock clock, IAppContext appContext, 
        ITokenRequestStorage tokenRequestStorage, JwtOptions jwtOptions)
    {
        _tokenService = tokenService;
        _clock = clock;
        _appContext = appContext;
        _tokenRequestStorage = tokenRequestStorage;
        _jwtOptions = jwtOptions;
    }
    
    public async Task HandleAsync(GenerateNewToken command, CancellationToken cancellationToken = default)
    {
        if (command.Expiration == 0)
        {
            throw new ArgumentNullException(nameof(command.Expiration));
        }
        if (command.Scopes == null)
        {
            throw new ArgumentNullException(nameof(command.Scopes));
        }

        var clientId = "pat_client";
        var issuer = _jwtOptions.ValidIssuer;
        var expiration = TimeSpan.FromDays(command.Expiration).Seconds;
        
        var scopes = command.Scopes.Select(s => new Claim(JwtClaimTypes.Scope, s));
        var userClaims = _appContext.Identity.Claims.Where(c => c.Key != JwtClaimTypes.Scope).Select(s => new Claim(s.Key, s.Value.FirstOrDefault() ?? string.Empty));
        
        var token = new Token
        {
            Audiences = new string[] { "my_api" },
            ClientId = clientId,
            CreationTime = _clock.UtcNow.UtcDateTime,
            Issuer = issuer,
            Lifetime = expiration,
            Type = OidcConstants.TokenTypes.AccessToken,
            AccessTokenType = AccessTokenType.Reference,
            Claims = new HashSet<Claim>(scopes.Concat(userClaims).ToArray(), new ClaimComparer())
        };

        var createdToken = await _tokenService.CreateSecurityTokenAsync(token);
        _tokenRequestStorage.SetToken(command.ReferenceId, createdToken);
    }
}