using System;
using Convey.CQRS.Queries;
using Spirebyte.Services.Identity.Application.Users.DTO;

namespace Spirebyte.Services.Identity.Application.Users.Queries;

public record GetUser(string UserId) : IQuery<UserDto>;