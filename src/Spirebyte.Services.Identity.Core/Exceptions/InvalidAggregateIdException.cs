using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class InvalidAggregateIdException : DomainException
{
    public InvalidAggregateIdException() : base("Invalid aggregate id.")
    {
    }

    public override string Code { get; } = "invalid_aggregate_id";
}