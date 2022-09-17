using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Identity.Application.Users.Exceptions;

public class SignUpException : AppException
{
    public SignUpException(string code, string description) : base($"Signup error [{code}] {description}")
    {
    }
}