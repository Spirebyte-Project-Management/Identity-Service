using System;
using Convey.CQRS.Queries;
using Spirebyte.Services.Identity.Application.Users.DTO;

namespace Spirebyte.Services.Identity.Application.Users.Queries;

public record GetUser(Guid UserId) : IQuery<UserDto>;