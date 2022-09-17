using System;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Identity.Application.Users.Exceptions;

public class UserNotFoundException : AppException
{
    public UserNotFoundException(Guid userId) : base($"User with ID: '{userId}' was not found.")
    {
        UserId = userId;
    }
    public Guid UserId { get; }
}