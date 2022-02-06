using System;
using Spirebyte.Services.Identity.Application.Exceptions.Base;

namespace Spirebyte.Services.Identity.Application.Users.Exceptions;

public class UserNotFoundException : AppException
{
    public UserNotFoundException(Guid userId) : base($"User with ID: '{userId}' was not found.")
    {
        UserId = userId;
    }

    public override string Code { get; } = "user_not_found";
    public Guid UserId { get; }
}