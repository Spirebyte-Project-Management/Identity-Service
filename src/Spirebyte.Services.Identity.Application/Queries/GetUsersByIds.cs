using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Spirebyte.Services.Identity.Application.DTO;

namespace Spirebyte.Services.Identity.Application.Queries;

public class GetUsersByIds : IQuery<IEnumerable<UserDto>>
{
    public GetUsersByIds(Guid[] userIds)
    {
        UserIds = userIds;
    }

    public Guid[] UserIds { get; set; }
}