﻿using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class EmailInUseException : DomainException
{
    public EmailInUseException(string email) : base($"Email {email} is already in use.")
    {
        Email = email;
    }

    public override string Code { get; } = "email_in_use";
    public string Email { get; }
}