using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.Commands;

[Contract]
public class ResetPassword : ICommand
{
    public Guid UserId;

    public ResetPassword(Guid userId, string password, string token)
    {
        UserId = userId;
        Password = password;
        Token = token;
    }

    public string Password { get; set; }
    public string Token { get; set; }
}