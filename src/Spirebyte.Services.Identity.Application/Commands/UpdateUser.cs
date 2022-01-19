using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.Commands;

[Contract]
public class UpdateUser : ICommand
{
    public UpdateUser(Guid userId, string fullname, string pic, string file)
    {
        UserId = userId == Guid.Empty ? Guid.NewGuid() : userId;
        Fullname = fullname;
        Pic = pic;
        File = file;
    }

    public Guid UserId { get; set; }
    public string Fullname { get; }
    public string Pic { get; }

    public string File { get; }
}