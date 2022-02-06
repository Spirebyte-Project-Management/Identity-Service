using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Identity.Application.Contexts;

public interface IIdentityContext
{
    bool IsAuthenticated { get; }
    public Guid Id { get; }
    string Role { get; }
    Dictionary<string, IEnumerable<string>> Claims { get; }
    bool IsUser();
    bool IsAdmin();
}