using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.Users.Commands;

[Contract]
public record ResetPassword(Guid UserId, string Password, string Token) : ICommand;