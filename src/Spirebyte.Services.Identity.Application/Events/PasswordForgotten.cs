using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;
using Spirebyte.Services.Identity.Core.Entities.Base;

namespace Spirebyte.Services.Identity.Application.Events
{
    [Contract]
    public class PasswordForgotten : IEvent
    {
        public Guid UserId { get; }
        public string Fullname { get; }
        public string Email { get; }
        public string Token { get; }

        public PasswordForgotten(Guid userId, string fullname, string email, string token)
        {
            UserId = userId;
            Fullname = fullname;
            Email = email;
            Token = token;
        }
    }
}
