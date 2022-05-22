using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Identity.Application.Users.Events;

[Contract]
[Message("identity")]
public class SignedUp : IEvent
{
    public SignedUp(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }

    public Guid UserId { get; }
    public string Email { get; }
}