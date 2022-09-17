using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class RevokedRefreshTokenException : DomainException
{
    public RevokedRefreshTokenException() : base("Revoked refresh token.")
    {
    }

    public string Code { get; } = "revoked_refresh_token";
}