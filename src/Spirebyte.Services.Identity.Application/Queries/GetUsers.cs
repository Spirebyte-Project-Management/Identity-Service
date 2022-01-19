using System.Collections.Generic;
using Convey.CQRS.Queries;
using Spirebyte.Services.Identity.Application.DTO;

namespace Spirebyte.Services.Identity.Application.Queries;

public class GetUsers : IQuery<IEnumerable<UserDto>>

{
    public GetUsers(string searchTerm = null)
    {
        SearchTerm = searchTerm;
    }

    public string SearchTerm { get; set; }
}