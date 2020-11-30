using Convey.CQRS.Commands;
using System;

namespace Spirebyte.Services.Identity.Application.Commands
{
    [Contract]
    public class UpdateUser : ICommand
    {
        public Guid UserId { get; set; }
        public string Fullname { get; }
        public string Pic { get; }

        public string File { get; }

        public UpdateUser(Guid userId, string fullname, string pic, string file)
        {
            UserId = userId == Guid.Empty ? Guid.NewGuid() : userId;
            Fullname = fullname;
            Pic = pic;
            File = file;
        }
    }
}
