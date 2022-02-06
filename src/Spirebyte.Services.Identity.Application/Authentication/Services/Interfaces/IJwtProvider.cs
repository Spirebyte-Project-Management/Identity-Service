using System;
using System.Collections.Generic;
using Spirebyte.Services.Identity.Application.Authentication.DTO;

namespace Spirebyte.Services.Identity.Application.Authentication.Services.Interfaces;

public interface IJwtProvider
{
    AuthDto Create(Guid userId, string role, string audience = null,
        IDictionary<string, IEnumerable<string>> claims = null);
}