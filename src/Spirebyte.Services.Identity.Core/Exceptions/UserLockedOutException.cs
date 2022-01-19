using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class UserLockedOutException : DomainException
{
    public UserLockedOutException(int hoursLeft) : base(
        $"User locked out try again in {hoursLeft + 1} hours. Or reset password.")
    {
    }

    public override string Code { get; } = "user_locked_out";
}