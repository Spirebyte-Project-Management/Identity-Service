using System;
using System.Collections.Generic;
using Convey.Types;

namespace Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;

internal sealed class UserDocument : IIdentifiable<Guid>
{
    public string Email { get; set; }
    public string Fullname { get; set; }
    public string NORMALIZED_FULLNAME { get; set; }
    public string Pic { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }
    public string SecurityStamp { get; set; }
    public DateTime CreatedAt { get; set; }
    public int AccessFailedCount { get; set; }
    public DateTime LockoutEnd { get; set; }
    public IEnumerable<string> Permissions { get; set; }
    public Guid Id { get; set; }
}