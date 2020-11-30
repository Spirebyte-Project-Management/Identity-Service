using Spirebyte.Services.Identity.Application.DTO;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Identity.Application.Services.Interfaces
{
    public interface IJwtProvider
    {
        AuthDto Create(Guid userId, string role, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null);
    }
}
