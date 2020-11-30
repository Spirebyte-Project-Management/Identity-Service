using Convey.CQRS.Queries;
using Spirebyte.Services.Identity.Application.DTO;
using System;

namespace Spirebyte.Services.Identity.Application.Queries
{
    public class GetUser : IQuery<UserDto>
    {
        public Guid UserId { get; set; }

        public GetUser(Guid userId)
        {
            UserId = userId;
        }
    }
}
