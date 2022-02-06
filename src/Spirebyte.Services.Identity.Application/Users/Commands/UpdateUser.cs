using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.Users.Commands;

[Contract]
public record UpdateUser(Guid UserId, string Fullname, string File) : ICommand;