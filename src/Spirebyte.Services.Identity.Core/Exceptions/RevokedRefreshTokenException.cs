using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions
{
    public class RevokedRefreshTokenException : DomainException
    {
        public override string Code { get; } = "revoked_refresh_token";

        public RevokedRefreshTokenException() : base("Revoked refresh token.")
        {
        }
    }
}
