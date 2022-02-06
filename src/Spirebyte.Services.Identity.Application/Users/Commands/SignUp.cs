using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.Users.Commands;

[Contract]
public record SignUp(Guid UserId, string Email, string Fullname, string Pic, string Password, string Role,
    IEnumerable<string> Permissions) : ICommand
{
    public Guid UserId { get; init; } = UserId == Guid.Empty ? Guid.NewGuid() : UserId;
    public string Role { get; init; } = Role == string.Empty ? "User" : Role;
}