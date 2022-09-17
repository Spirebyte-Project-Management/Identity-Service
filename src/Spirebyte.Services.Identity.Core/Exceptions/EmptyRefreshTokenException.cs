using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class EmptyRefreshTokenException : DomainException
{
    public EmptyRefreshTokenException() : base("Empty refresh token.")
    {
    }

    public string Code { get; } = "empty_refresh_token";
}