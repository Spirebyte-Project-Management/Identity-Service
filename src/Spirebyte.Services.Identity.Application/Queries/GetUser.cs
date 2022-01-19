using System;
using Convey.CQRS.Queries;
using Spirebyte.Services.Identity.Application.DTO;

namespace Spirebyte.Services.Identity.Application.Queries;

public class GetUser : IQuery<UserDto>
{
    public GetUser(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; set; }
}