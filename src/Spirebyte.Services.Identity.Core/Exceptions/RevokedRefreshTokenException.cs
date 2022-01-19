using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class RevokedRefreshTokenException : DomainException
{
    public RevokedRefreshTokenException() : base("Revoked refresh token.")
    {
    }

    public override string Code { get; } = "revoked_refresh_token";
}