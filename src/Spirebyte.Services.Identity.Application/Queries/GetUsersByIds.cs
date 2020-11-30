using Convey.CQRS.Queries;
using Spirebyte.Services.Identity.Application.DTO;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Identity.Application.Queries
{
    public class GetUsersByIds : IQuery<IEnumerable<UserDto>>
    {
        public Guid[] UserIds { get; set; }

        public GetUsersByIds(Guid[] userIds)
        {
            UserIds = userIds;
        }
    }
}
