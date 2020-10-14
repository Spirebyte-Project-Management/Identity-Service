using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.Commands
{
    [Contract]
    public class ForgotPassword : ICommand
    {
        public string Email { get; set; }
    }
}
