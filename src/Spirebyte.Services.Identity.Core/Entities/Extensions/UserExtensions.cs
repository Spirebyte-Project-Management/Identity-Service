using System.Security.Claims;
using System.Security.Principal;

namespace Spirebyte.Services.Identity.Core.Entities.Extensions;

public static class UserExtensions
{
    public static string Pic(this IIdentity identity)
    {
        var claim = ((ClaimsIdentity)identity).FindFirst(c => c.Type == nameof(User.Pic));
        // Test for null to avoid issues during local testing
        return claim != null ? claim.Value : string.Empty;
    }
}