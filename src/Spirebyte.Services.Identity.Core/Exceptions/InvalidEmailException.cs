using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException(string email) : base($"Invalid email: {email}.")
    {
    }

    public string Code { get; } = "invalid_email";
}