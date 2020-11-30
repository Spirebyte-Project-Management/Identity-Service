using Convey.CQRS.Commands;
using System;

namespace Spirebyte.Services.Identity.Application.Commands
{
    [Contract]
    public class ResetPassword : ICommand
    {
        public Guid UserId;
        public string Password { get; set; }
        public string Token { get; set; }

        public ResetPassword(Guid userId, string password, string token)
        {
            UserId = userId;
            Password = password;
            Token = token;
        }
    }
}
