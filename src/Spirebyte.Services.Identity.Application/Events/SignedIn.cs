using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Identity.Application.Events;

[Contract]
public class SignedIn : IEvent
{
    public SignedIn(Guid userId, string role)
    {
        UserId = userId;
        Role = role;
    }

    public Guid UserId { get; }
    public string Role { get; }
}