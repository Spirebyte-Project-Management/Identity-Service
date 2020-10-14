using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Commands;
using Spirebyte.Services.Identity.Core.Entities.Base;

namespace Spirebyte.Services.Identity.Application.Commands
{
    [Contract]
    public class ResetPassword : ICommand
    {
        public AggregateId UserId;
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
