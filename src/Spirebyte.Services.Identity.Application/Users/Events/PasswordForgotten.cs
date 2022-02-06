using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Identity.Application.Users.Events;

[Contract]
public record PasswordForgotten(Guid UserId, string Fullname, string Email, string Token) : IEvent;