using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class InvalidRefreshTokenException : DomainException
{
    public InvalidRefreshTokenException() : base("Invalid refresh token.")
    {
    }

    public override string Code { get; } = "invalid_refresh_token";
}