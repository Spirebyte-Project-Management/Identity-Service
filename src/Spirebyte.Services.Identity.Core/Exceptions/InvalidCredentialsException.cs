using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException(string email) : base("Invalid credentials.")
    {
        Email = email;
    }

    public override string Code { get; } = "invalid_credentials";
    public string Email { get; }
}