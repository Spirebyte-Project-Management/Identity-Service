using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class UserLockedOutException : DomainException
{
    public UserLockedOutException(int hoursLeft) : base(
        $"User locked out try again in {hoursLeft + 1} hours. Or reset password.")
    {
    }

    public string Code { get; } = "user_locked_out";
}