using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;
using Spirebyte.Services.Identity.Application.DTO;

namespace Spirebyte.Services.Identity.Application.Queries
{
    public class GetUsers : IQuery<IEnumerable<UserDto>>

    {
        public string SearchTerm { get; set; }
    }
}
