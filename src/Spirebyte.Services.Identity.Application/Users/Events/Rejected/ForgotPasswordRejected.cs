using Convey.CQRS.Events;

namespace Spirebyte.Services.Identity.Application.Users.Events.Rejected;

[Contract]
public class ForgotPasswordRejected : IRejectedEvent
{
    public ForgotPasswordRejected(string email, string reason, string code)
    {
        Email = email;
        Reason = reason;
        Code = code;
    }

    public string Email { get; }
    public string Reason { get; }
    public string Code { get; }
}