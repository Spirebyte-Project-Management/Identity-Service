using System;
using System.Collections.Generic;
using System.Text;
using Convey.Types;

namespace Spirebyte.Services.Identity.Infrastructure.Mongo.Documents
{
    internal sealed class RefreshTokenDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
    }
}
