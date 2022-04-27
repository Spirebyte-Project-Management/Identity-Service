﻿using Convey.CQRS.Events;

namespace Spirebyte.Services.Identity.Application.Authentication.Events.Rejected;

[Contract]
public class SignInRejected : IRejectedEvent
{
    public SignInRejected(string email, string reason, string code)
    {
        Email = email;
        Reason = reason;
        Code = code;
    }

    public string Email { get; }
    public string Reason { get; }
    public string Code { get; }
}