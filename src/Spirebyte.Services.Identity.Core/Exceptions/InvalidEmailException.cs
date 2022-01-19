using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException(string email) : base($"Invalid email: {email}.")
    {
    }

    public override string Code { get; } = "invalid_email";
}