using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Identity.Application.Authentication.Events;

[Contract]
public record SignedIn(Guid UserId, string Role) : IEvent;