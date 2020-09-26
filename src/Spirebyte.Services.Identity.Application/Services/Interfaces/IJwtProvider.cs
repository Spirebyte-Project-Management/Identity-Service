using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Identity.Application.DTO;

namespace Spirebyte.Services.Identity.Application.Services
{
    public interface IJwtProvider
    {
        AuthDto Create(Guid userId, string role, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null);
    }
}
