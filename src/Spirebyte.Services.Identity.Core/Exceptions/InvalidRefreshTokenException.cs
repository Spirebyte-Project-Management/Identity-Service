using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class InvalidRefreshTokenException : DomainException
{
    public InvalidRefreshTokenException() : base("Invalid refresh token.")
    {
    }

    public string Code { get; } = "invalid_refresh_token";
}