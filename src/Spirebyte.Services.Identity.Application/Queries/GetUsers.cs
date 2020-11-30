using Convey.CQRS.Queries;
using Spirebyte.Services.Identity.Application.DTO;
using System.Collections.Generic;

namespace Spirebyte.Services.Identity.Application.Queries
{
    public class GetUsers : IQuery<IEnumerable<UserDto>>

    {
        public string SearchTerm { get; set; }

        public GetUsers(string searchTerm = null)
        {
            SearchTerm = searchTerm;
        }
    }
}
