using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class InvalidFullnameException : DomainException
{
    public InvalidFullnameException(string fullname) : base($"Invalid fullname: {fullname}.")
    {
    }

    public string Code { get; } = "invalid_fullname";
}