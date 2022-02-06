using System;
using Spirebyte.Services.Identity.Application.Exceptions.Base;

namespace Spirebyte.Services.Identity.Application.Authentication.Exceptions;

public class InvalidTokenException : AppException
{
    public InvalidTokenException(Guid userId) : base($"Token for user with id: '{userId}' was invalid.")
    {
        UserId = userId;
    }

    public override string Code { get; } = "invalid_token";
    public Guid UserId { get; }
}