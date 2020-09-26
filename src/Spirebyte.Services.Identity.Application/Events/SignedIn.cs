using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Identity.Application.Events
{
    [Contract]
    public class SignedIn : IEvent
    {
        public Guid UserId { get; }
        public string Role { get; }

        public SignedIn(Guid userId, string role)
        {
            UserId = userId;
            Role = role;
        }
    }
}
