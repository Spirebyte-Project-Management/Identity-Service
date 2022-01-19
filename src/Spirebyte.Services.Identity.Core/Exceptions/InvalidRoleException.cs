using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class InvalidRoleException : DomainException
{
    public InvalidRoleException(string role) : base($"Invalid role: {role}.")
    {
    }

    public override string Code { get; } = "invalid_role";
}