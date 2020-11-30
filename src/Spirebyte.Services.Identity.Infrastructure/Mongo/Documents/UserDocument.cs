using Convey.Types;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Identity.Infrastructure.Mongo.Documents
{
    internal sealed class UserDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string NORMALIZED_FULLNAME { get; set; }
        public string Pic { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string SecurityStamp { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
