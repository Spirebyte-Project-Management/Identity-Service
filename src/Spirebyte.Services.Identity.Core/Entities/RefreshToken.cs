﻿using System;
using Spirebyte.Services.Identity.Core.Entities.Base;
using Spirebyte.Services.Identity.Core.Exceptions;

namespace Spirebyte.Services.Identity.Core.Entities;

public class RefreshToken : AggregateRoot
{
    protected RefreshToken()
    {
    }

    public RefreshToken(AggregateId id, AggregateId userId, string token, DateTime createdAt,
        DateTime? revokedAt = null)
    {
        if (string.IsNullOrWhiteSpace(token)) throw new EmptyRefreshTokenException();

        Id = id;
        UserId = userId;
        Token = token;
        CreatedAt = createdAt;
        RevokedAt = revokedAt;
    }

    public AggregateId UserId { get; }
    public string Token { get; }
    public DateTime CreatedAt { get; }
    public DateTime? RevokedAt { get; private set; }
    public bool Revoked => RevokedAt.HasValue;

    public void Revoke(DateTime revokedAt)
    {
        if (Revoked) throw new RevokedRefreshTokenException();

        RevokedAt = revokedAt;
    }
}