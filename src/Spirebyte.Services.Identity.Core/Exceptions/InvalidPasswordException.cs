using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class InvalidPasswordException : DomainException
{
    public InvalidPasswordException() : base("Invalid password.")
    {
    }

    public string Code { get; } = "invalid_password";
}