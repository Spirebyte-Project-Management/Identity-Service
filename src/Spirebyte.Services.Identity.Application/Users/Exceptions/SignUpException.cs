using Spirebyte.Services.Identity.Application.Exceptions.Base;

namespace Spirebyte.Services.Identity.Application.Users.Exceptions;

public class SignUpException : AppException
{
    public SignUpException(string code, string description) : base($"Signup error [{code}] {description}")
    {
    }

    public override string Code { get; } = "signup error";
}