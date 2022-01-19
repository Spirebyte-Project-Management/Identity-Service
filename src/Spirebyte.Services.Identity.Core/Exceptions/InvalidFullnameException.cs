using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class InvalidFullnameException : DomainException
{
    public InvalidFullnameException(string fullname) : base($"Invalid fullname: {fullname}.")
    {
    }

    public override string Code { get; } = "invalid_fullname";
}