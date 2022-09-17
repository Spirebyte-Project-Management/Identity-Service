using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Identity.Application.Users.DTO;

namespace Spirebyte.Services.Identity.Application.Users.Queries;

public class GetUsersByIds : IQuery<IEnumerable<UserDto>>
{
    [JsonConstructor]
    public GetUsersByIds()
    {
    }

    public GetUsersByIds(Guid[] userIds = null)
    {
        UserIds = userIds;
    }

    public Guid[] UserIds { get; init; }
}