using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class InvalidRoleException : DomainException
{
    public InvalidRoleException(string role) : base($"Invalid role: {role}.")
    {
    }

    public string Code { get; } = "invalid_role";
}