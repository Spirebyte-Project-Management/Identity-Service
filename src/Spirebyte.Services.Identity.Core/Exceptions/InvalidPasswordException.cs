using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class InvalidPasswordException : DomainException
{
    public InvalidPasswordException() : base("Invalid password.")
    {
    }

    public override string Code { get; } = "invalid_password";
}