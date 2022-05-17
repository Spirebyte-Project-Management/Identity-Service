using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Identity.Application.Users.Events;

[Contract]
public record SignedUp(Guid UserId, string Email) : IEvent;