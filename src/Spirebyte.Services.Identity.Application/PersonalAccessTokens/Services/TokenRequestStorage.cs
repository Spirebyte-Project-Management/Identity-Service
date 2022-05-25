using System;
using Microsoft.Extensions.Caching.Memory;
using Spirebyte.Services.Identity.Application.PersonalAccessTokens.Services.Interfaces;

namespace Spirebyte.Services.Identity.Application.PersonalAccessTokens.Services;

public class TokenRequestStorage : ITokenRequestStorage
{
    private readonly IMemoryCache _cache;

    public TokenRequestStorage(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void SetToken(Guid referenceId, string token)
    {
        _cache.Set(GetKey(referenceId), token, TimeSpan.FromSeconds(5));
    }

    public string GetToken(Guid referenceId)
    {
        return _cache.Get<string>(GetKey(referenceId));
    }

    private static string GetKey(Guid commandId)
    {
        return $"token:{commandId:N}";
    }
}