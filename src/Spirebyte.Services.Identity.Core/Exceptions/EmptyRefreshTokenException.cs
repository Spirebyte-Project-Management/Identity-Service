﻿using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions;

public class EmptyRefreshTokenException : DomainException
{
    public EmptyRefreshTokenException() : base("Empty refresh token.")
    {
    }

    public override string Code { get; } = "empty_refresh_token";
}