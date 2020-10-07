using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.Commands
{
    [Contract]
    public class UpdateUser : ICommand
    {
        public Guid UserId { get; set; }
        public string Fullname { get; }
        public string Pic { get; }

        public UpdateUser(Guid userId, string fullname, string pic)
        {
            UserId = userId == Guid.Empty ? Guid.NewGuid() : userId;
            Fullname = fullname;
            Pic = pic;
        }
    }
}
