using System;
using Convey.Types;

namespace Spirebyte.Services.Identity.Infrastructure.Mongo.Documents;

internal sealed class RefreshTokenDocument : IIdentifiable<Guid>
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public Guid Id { get; set; }
}