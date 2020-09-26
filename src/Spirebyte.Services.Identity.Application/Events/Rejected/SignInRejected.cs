using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Identity.Application.Events.Rejected
{
    [Contract]
    public class SignInRejected : IRejectedEvent
    {
        public string Email { get; }
        public string Reason { get; }
        public string Code { get; }

        public SignInRejected(string email, string reason, string code)
        {
            Email = email;
            Reason = reason;
            Code = code;
        }
    }
}
