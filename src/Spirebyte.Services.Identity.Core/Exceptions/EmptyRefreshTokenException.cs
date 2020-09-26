using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Identity.Core.Exceptions.Base;

namespace Spirebyte.Services.Identity.Core.Exceptions
{
    public class EmptyRefreshTokenException : DomainException
    {
        public override string Code { get; } = "empty_refresh_token";

        public EmptyRefreshTokenException() : base("Empty refresh token.")
        {
        }
    }
}
