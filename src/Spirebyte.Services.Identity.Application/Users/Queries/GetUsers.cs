using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Identity.Application.Users.DTO;

namespace Spirebyte.Services.Identity.Application.Users.Queries;

public record GetUsers(string SearchTerm = null) : IQuery<IEnumerable<UserDto>>;