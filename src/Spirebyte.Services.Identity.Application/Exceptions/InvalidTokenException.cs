﻿using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Identity.Application.Exceptions.Base;

namespace Spirebyte.Services.Identity.Application.Exceptions
{
    public class InvalidTokenException : AppException
    {
        public override string Code { get; } = "invalid_token";
        public Guid UserId { get; }

        public InvalidTokenException(Guid userId) : base($"Token for user with id: '{userId}' was invalid.")
        {
            UserId = userId;
        }
    }
}
